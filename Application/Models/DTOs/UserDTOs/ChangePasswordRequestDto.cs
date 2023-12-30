using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.UserDTOs
{
    public class ChangePasswordRequestDto
    {
        public string OldPassword { get; set; } = string.Empty;


        [MinLength(8, ErrorMessage = "password length must be greater or equal 8 character")]
        public string NewPassword { get; set; } = string.Empty;


        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
