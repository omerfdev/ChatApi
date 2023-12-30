using Domain.Entities;
using Domain.Repositories;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ChatContext context;
        public UserRepository(ChatContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(User user)
        {
            await context.Users.AddAsync(user);
        }

        public void Delete(User user)
        {
            context.Users.Remove(user);
        }

        public async Task<Tuple<List<User>, int>> GetUsers(
            int pageNumber,
            int pageSize,
            string searchText = null)
        {
            var users = context.Users.Include(c => c.Image).AsQueryable();
            if (!searchText.IsNullOrEmpty())
                users = users.Where(u => u.Username.Contains(searchText));

            var usersCount = await users.CountAsync();
            var numOfPages = Math.Ceiling(usersCount / (pageSize * 1f));
            if (pageNumber > numOfPages && numOfPages != 0)
            {
                return null;
            }
            var usersList = await users.OrderBy(c => c.Username).Skip((pageNumber - 1) * pageSize).Take(pageSize)
               .ToListAsync();
            var result = Tuple.Create(usersList, (int)numOfPages);
            return result;
        }

        public async Task<User?> GetUserById(int userId)
        {
            return await context.Users
                .Where(c => c.Id == userId)
                .Include(c => c.Image)
                .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await context.Users
                .Where(c => c.Username == username)
                .Include(c => c.Image)
                .FirstOrDefaultAsync();
        }

        public bool CheckIfUsernameExists(string username)
        {
            return context.Users.Any(u => u.Username == username);
        }

    }
}
