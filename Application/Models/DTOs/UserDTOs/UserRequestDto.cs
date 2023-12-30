using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.UserDTOs
{
    public class UserRequestDto
    {
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9_]{4,17}$", ErrorMessage = "username must be Aa9_")]
        public string Username { get; set; } = string.Empty;


        [MinLength(8, ErrorMessage = "password length must be greater or equal 8 character")]
        public string Password { get; set; } = string.Empty;
    }
}
