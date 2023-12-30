using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IPrivateMessageRepository
    {
        Task AddAsync(PrivateMessage message);
        void Delete(PrivateMessage message);
        Task<Tuple<List<PrivateMessage>, bool>> GetPrivateMessagesForPrivateChat(
            DateTime pageDate,
            int pageSize,
            int firstUserId,
            int secoundUserId);

        Task<IEnumerable<ChatWithLastMessage>> GetRecentChatsForUser(int userId);
    }
}
