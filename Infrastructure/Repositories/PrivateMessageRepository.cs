using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.DbContexts;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PrivateMessageRepository : IPrivateMessageRepository
    {
        private readonly IMongoCollection<PrivateMessage> _privateMessages;
        private readonly IMongoCollection<User> _users;


        public PrivateMessageRepository(IUserDatabaseSettings settings,
           IMongoClient mongoClient, IPrivateMessageDatabaseSettings settingsPrivateMessage)
        {
            var user_database = mongoClient.GetDatabase(settings.DatabaseName);
            var private_message_database = mongoClient.GetDatabase(settingsPrivateMessage.DatabaseName);

            _users = user_database.GetCollection<User>(settings.UserCollectionName);
            _privateMessages = private_message_database.GetCollection<PrivateMessage>(settingsPrivateMessage.PrivateMessageCollectionName);

        }

        public async Task AddAsync(PrivateMessage message)
        {
            await _privateMessages.InsertOneAsync(message);
        }

        public void Delete(PrivateMessage message)
        {
            var filter = Builders<PrivateMessage>.Filter.Eq(m => m.Id, message.Id);
            _privateMessages.DeleteOne(filter);
        }

        public async Task<Tuple<List<PrivateMessage>, bool>> GetPrivateMessagesForPrivateChat(
            DateTime pageDate,
            int pageSize,
            string firstUserId,
            string secondUserId)
        {
            var filter = Builders<PrivateMessage>.Filter.Where(m =>
                (m.SenderId == firstUserId && m.ReceiverId == secondUserId) ||
                (m.SenderId == secondUserId && m.ReceiverId == firstUserId) &&
                m.CreationDate < pageDate);

            var messagesCount = await _privateMessages.CountDocumentsAsync(filter);
            var isThereMore = messagesCount > pageSize;

            var messagesList = await _privateMessages
                .Find(filter)
                .SortBy(c => c.CreationDate) 
                .Skip((int)messagesCount - pageSize) 
                .ToListAsync();

            var result = Tuple.Create(messagesList, isThereMore);
            return result;
        }



        public async Task<IEnumerable<ChatWithLastMessage>> GetRecentChatsForUser(string userId)
        {
            var recentChatsWithLastMessages = await _privateMessages
                .Aggregate()
                .Match(m => m.SenderId == userId || m.ReceiverId == userId)
                .Group(g => g.SenderId == userId ? g.ReceiverId : g.SenderId, group => new
                {
                    User = group.Key,
                    LastMessage = group.OrderByDescending(msg => msg.CreationDate).First()
                })
                .SortByDescending(result => result.LastMessage.CreationDate)
                
                .ToListAsync();

            var result = await Task.WhenAll(recentChatsWithLastMessages.Select(async chat =>
            {
                var user = await _users.Find(u => u.Id == chat.User).FirstOrDefaultAsync();

                return new ChatWithLastMessage
                {
                    User = user,
                    LastMessage = chat.LastMessage
                };
            }));

            return result;
        }




    }
}
