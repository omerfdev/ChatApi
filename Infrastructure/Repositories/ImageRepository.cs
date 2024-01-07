using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Models;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IMongoCollection<Images> _images;
        private readonly IMongoCollection<User> _user;

        public ImageRepository(IImageDatabaseSettings settings, IMongoClient mongoClient, IUserDatabaseSettings user)
        {
            var imagesDatabase = mongoClient.GetDatabase(settings.DatabaseName);
            _images = imagesDatabase.GetCollection<Images>(settings.ImageCollectionName);
            var userDatabase = mongoClient.GetDatabase(user.DatabaseName);
            _user = userDatabase.GetCollection<User>(user.UserCollectionName);
            
        }

        public async Task AddAsync(Images images)
        {
            await _images.InsertOneAsync(images);

            //await _user.UpdateOneAsync(images.userId);
        }

        public async Task DeleteAsync(string imagesId)
        {
            var filter = Builders<Images>.Filter.Eq(x => x.Id, imagesId);
            await _images.DeleteOneAsync(filter);
        }

        public async Task<Images> GetByIdAsync(string imagesId)
        {
            var filter = Builders<Images>.Filter.Eq(x => x.Id, imagesId);
            return await _images.Find(filter).FirstOrDefaultAsync();
        }
    }
}
