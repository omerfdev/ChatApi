using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        bool CheckIfUsernameExists(string username);
        void Delete(User user);
        Task<User?> GetUserById(string userId);
        Task<Tuple<List<User>, int>> GetUsers(int pageNumber, int pageSize, string searchText = null);
        Task<User?> GetUserByUsername(string username);
    }
}
