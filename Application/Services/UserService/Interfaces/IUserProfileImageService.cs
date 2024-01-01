using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserService.Interfaces
{
    public interface IUserProfileImageService
    {
        Task AddProfilePictureAsync(string userId, IFormFile image);
        Task ChangeProfilePictureAsync(string userId, IFormFile image);
        Task DeleteProfilePictureAsync(string userId);
    }
}
