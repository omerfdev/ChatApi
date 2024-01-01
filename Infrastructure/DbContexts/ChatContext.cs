using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DbContexts
{
    public class ChatContext 
    {
        private readonly IMongoDatabase _database;

        public IMongoCollection<User> User { get; }
        public IMongoCollection<PrivateMessage> PrivateMessage { get; }
        public IMongoCollection<Domain.Entities.Image> Image { get; }

        public ChatContext(IMongoDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            User = _database.GetCollection<User>("User");
            PrivateMessage = _database.GetCollection<PrivateMessage>("PrivateMessage");
            Image = _database.GetCollection<Domain.Entities.Image>("Image");
        }
    }
}
