using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
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
            string firstUserId,
            string secondUserId);

        Task<IEnumerable<ChatWithLastMessage>> GetRecentChatsForUser(string userId);
    }
}
