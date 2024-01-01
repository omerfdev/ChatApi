using Application.Models.DTOs.UserDTOs;
using Application.Services.UserService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRetrievalService userRetrievalService;
        private readonly IUserAccountService userAccountService;


        public UsersController(IUserRetrievalService userRetrievalService, IUserAccountService userAccountService)
        {
            this.userRetrievalService = userRetrievalService;
            this.userAccountService = userAccountService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers(
            int pageNumber = 1,
            int pageSize = 10,
            string searchText = null)
        {
            var users = await userRetrievalService.GetUsers(pageNumber, pageSize, searchText);
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await userRetrievalService.GetUserById(userId);
            return Ok(user);
        }

        [Authorize]
        [HttpPut("{userId}/change-password")]
        public async Task<IActionResult> ChangePassword([FromRoute] string userId, [FromBody] ChangePasswordRequestDto changePasswordDto)
        {
            await userAccountService.ChangePasswordAsync(userId, changePasswordDto);
            return NoContent();
        }

        [Authorize]
        [HttpPut("{userId}/about")]
        public async Task<IActionResult> ChangeUserAbout([FromRoute] string userId, [FromQuery] string newAbout)
        {
            await userAccountService.ChangeUserAboutAsync(userId, newAbout);
            return NoContent();
        }

        [Authorize]
        [HttpPut("{userId}/dark-mode")]
        public async Task<IActionResult> ChangeThemeMode([FromRoute] string userId, [FromQuery] bool isDarkMode)
        {
            await userAccountService.ChangeThemeModeAsync(userId, isDarkMode);
            return NoContent();
        }
    }
}
