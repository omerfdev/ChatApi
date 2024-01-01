using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DbContexts
{
    public class ChatContext
    {
        private readonly IMongoDatabase _database;
      
        public ChatContext(IConfiguration configuration)
        {
            string connectionUri = "mongodb+srv://omerfdev:Admin1234@cluster0.xsev4x8.mongodb.net/?retryWrites=true&w=majority";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);

            // Set the ServerApi field of the settings object to Stable API version 1
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            // Create a new client and connect to the server
            var client = new MongoClient(settings);

            // Send a ping to confirm a successful connection
            try
            {
                var result = client.GetDatabase("omerfdev").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw; // Handle the exception as needed
            }

            string databaseName = "ChatApps";
            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw new ArgumentException("MongoDBSettings:DatabaseName cannot be null or empty.");
            }

            _database = client.GetDatabase(databaseName);

            EnsureCollectionsExist();

            Console.WriteLine("Connected to MongoDB");
        }


        private void EnsureCollectionsExist()
        {
            EnsureCollectionExists("User");
            EnsureCollectionExists("PrivateMessage");
            EnsureCollectionExists("Image");
            EnsureCollectionExists("ConnectionInfo");
        }

        private void EnsureCollectionExists(string collectionName)
        {
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            if (!CollectionExists(collection))
            {
                _database.CreateCollection(collectionName);
            }
        }

        private bool CollectionExists(IMongoCollection<BsonDocument> collection)
        {
            var filter = new BsonDocument();
            var collectionName = collection.CollectionNamespace.CollectionName;

            var collectionNames = _database.ListCollectionNames(new ListCollectionNamesOptions
            {
                Filter = Builders<BsonDocument>.Filter.Eq("name", collectionName)
            });

            return collectionNames.Any();
        }

        public IMongoDatabase GetDatabase()
        {
            return _database;
        }
    }
}
