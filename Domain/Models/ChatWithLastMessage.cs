using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ChatWithLastMessage
    {
        public User User { get; set; }
        public PrivateMessage LastMessage { get; set; }
    }
}
