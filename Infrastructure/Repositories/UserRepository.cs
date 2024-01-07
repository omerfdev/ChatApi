using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.DbContexts;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IUserDatabaseSettings settings,
           IMongoClient mongoClient)  // Add ChatContext parameter
        {
            var user_database = mongoClient.GetDatabase(settings.DatabaseName);           

            _users = user_database.GetCollection<User>(settings.UserCollectionName);
            
        }


        public async Task AddAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        public void Delete(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            _users.DeleteOne(filter);
        }

        public async Task<Tuple<List<User>, int>> GetUsers(
            int pageNumber,
            int pageSize,
            string searchText = null)
        {
            var filterBuilder = Builders<User>.Filter;
            var filterDefinition = filterBuilder.Empty; 

            if (!string.IsNullOrEmpty(searchText))
            {
                filterDefinition &= filterBuilder.Regex("Username", new MongoDB.Bson.BsonRegularExpression(searchText, "i"));
            }

            var users = await _users
                .Find(filterDefinition)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            var usersCount = await _users.CountDocumentsAsync(filterDefinition);
            var numOfPages = (int)Math.Ceiling(usersCount / (pageSize * 1.0));

            var result = Tuple.Create(users, numOfPages);
            return result;
        }

        public async Task<User?> GetUserById(string userId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            return await _users.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Username, username);
            return await _users.Find(filter).FirstOrDefaultAsync();
        }

        public bool CheckIfUsernameExists(string username)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Username, username);
            return _users.Find(filter).Any();
        }
    }
}
