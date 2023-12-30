using Domain.Repositories;
using Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatContext context;
        public UnitOfWork(ChatContext context)
        {
            this.context = context;
        }

        public async Task<int> SaveChangesAsync()
        {

            return await context.SaveChangesAsync();
        }
    }
}
