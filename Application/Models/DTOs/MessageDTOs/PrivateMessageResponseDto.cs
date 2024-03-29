﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.MessageDTOs
{
    public class PrivateMessageResponseDto
    {
        public int Id { get; set; }
        public string TextBody { get; set; }
        public DateTime CreationDate { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}
