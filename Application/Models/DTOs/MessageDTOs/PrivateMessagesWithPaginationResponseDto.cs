using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.MessageDTOs
{
    public class PrivateMessagesWithPaginationResponseDto
    {
        public IEnumerable<PrivateMessageResponseDto> Messages { get; set; }
        public bool IsThereMore { get; set; }
    }
}
