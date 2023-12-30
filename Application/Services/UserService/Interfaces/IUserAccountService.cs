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
        Task ChangePasswordAsync(int userId, ChangePasswordRequestDto changePasswordDto);
        Task ChangeUserAboutAsync(int userId, string newAbout);
        Task ChangeThemeModeAsync(int userId, bool isDarkMode);
    }
}
