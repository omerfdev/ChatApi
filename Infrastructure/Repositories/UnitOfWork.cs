﻿using Domain.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoDatabase _database;
        private readonly IClientSessionHandle _session;

        public UnitOfWork(IMongoDatabase database, IClientSessionHandle session)
        {
            _database = database;
            _session = session;
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                await _session.CommitTransactionAsync();
                return 1; // Success
            }
            catch (MongoException)
            {
                await _session.AbortTransactionAsync();
                return 0; // Failure
            }
            finally
            {
                _session.Dispose();
            }
        }
    }
}
