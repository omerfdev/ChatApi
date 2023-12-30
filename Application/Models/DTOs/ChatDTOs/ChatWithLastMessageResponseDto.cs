using Application.Models.DTOs.MessageDTOs;
using Application.Models.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.ChatDTOs
{
    public class ChatWithLastMessageResponseDto
    {
        public UserResponseDto User { get; set; }
        public PrivateMessageResponseDto LastMessage { get; set; }
    }
}
