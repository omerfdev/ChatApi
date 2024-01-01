using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.MessageDTOs
{
    public class PrivateMessageResponseDto
    {
        public string Id { get; set; }
        public string TextBody { get; set; }
        public DateTime CreationDate { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
    }
}
