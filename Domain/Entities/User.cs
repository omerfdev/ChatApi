using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string About { get; set; } = string.Empty;
        public bool IsDarkTheme { get; set; } = false;
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public int? ImageId { get; set; }
        public Image Image { get; set; }
        public ICollection<PrivateMessage> SendedPrivateMessages { get; set; }
        public ICollection<PrivateMessage> ReceivedPrivateMessages { get; set; }
    }
}
