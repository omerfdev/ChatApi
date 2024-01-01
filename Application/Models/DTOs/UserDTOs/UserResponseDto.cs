using Application.Models.DTOs.ImageDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.UserDTOs
{
    public class UserResponseDto
    {
        public string Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string About { get; set; } = string.Empty;
        public bool IsDarkTheme { get; set; }
        public ImageResponseDto Image { get; set; }
    }
}
