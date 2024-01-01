using Application.Models.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserService.Interfaces
{
    public interface IUserAccountService
    {
        Task<string> LoginUserAsync(UserRequestDto userRequestDto);
        Task<UserResponseDto> RegisterUserAsync(UserRequestDto userRequestDto);
        Task<UserResponseDto> GetUserByJwtTokenAsync();
        Task ChangePasswordAsync(string userId, ChangePasswordRequestDto changePasswordDto);
        Task ChangeUserAboutAsync(string userId, string newAbout);
        Task ChangeThemeModeAsync(string userId, bool isDarkMode);
    }
}
