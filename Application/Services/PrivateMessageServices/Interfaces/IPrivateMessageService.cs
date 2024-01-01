using Application.Models.DTOs.ChatDTOs;
using Application.Models.DTOs.MessageDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.PrivateMessageServices.Interfaces
{
    public interface IPrivateMessageService
    {
        Task<PrivateMessageResponseDto> StorePrivateMessage(string destinationUserId, string textMessage);
        Task<PrivateMessagesWithPaginationResponseDto> GetPrivateMessages(
            DateTime? pageDate,
            int pageSize,
            string firstUserId,
            string secoundUserId);
        Task<IEnumerable<ChatWithLastMessageResponseDto>> GetRecentChatsForUser(string userId);
    }
}
