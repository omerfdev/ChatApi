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
           IMongoClient mongoClient, IPrivateMessageDatabaseSettings settingsPrivateMessage)  // Add ChatContext parameter
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
                .SortByDescending(c => c.CreationDate)
                .Limit(pageSize)
                .SortBy(c => c.CreationDate)
                .ToListAsync();

            var result = Tuple.Create(messagesList, isThereMore);
            return result;
        }

        public async Task<IEnumerable<ChatWithLastMessage>> GetRecentChatsForUser(string userId)
        {
            var recentChatsWithLastMessages = await _privateMessages
                .AsQueryable()
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                .OrderByDescending(g => g.Max(m => m.CreationDate))
                .Take(10)
                .ToListAsync();

            var result = recentChatsWithLastMessages.Select(async g =>
            {
                var user = await _users.Find(u => u.Id == g.Key).FirstOrDefaultAsync();
                var lastMessage = g.OrderByDescending(msg => msg.CreationDate).First();

                return new ChatWithLastMessage
                {
                    User = user,
                    LastMessage = lastMessage
                };
            }).ToList();

            return await Task.WhenAll(result);
        }

    }
}
