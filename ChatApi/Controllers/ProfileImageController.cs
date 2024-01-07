using Application.Services.UserService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChatApi.Controllers
{
    [Route("api/users/{userId}/profile-image")]
    [ApiController]
    public class ProfileImageController : ControllerBase
    {
        private readonly IUserProfileImageService userProfileImageService;

        public ProfileImageController(IUserProfileImageService userProfileImageService)
        {
            this.userProfileImageService = userProfileImageService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProfilePicture([FromRoute] string userId, IFormFile image)
        {
            try
            {
                await userProfileImageService.AddProfilePictureAsync(userId, image);
                return Ok("Success: Profile picture added.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> ChangeProfilePicture([FromRoute] string userId, IFormFile image)
        {
            try
            {
                await userProfileImageService.ChangeProfilePictureAsync(userId, image);
                return Ok("Success: Profile picture changed.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteProfilePicture([FromRoute] string userId)
        {
            try
            {
                await userProfileImageService.DeleteProfilePictureAsync(userId);
                return Ok("Success: Profile picture deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
