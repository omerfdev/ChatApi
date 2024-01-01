﻿using Application.Services.UserService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            await userProfileImageService.AddProfilePictureAsync(userId, image);
            return Ok("success");
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> ChangeProfilePicture([FromRoute] string userId, IFormFile image)
        {
            await userProfileImageService.ChangeProfilePictureAsync(userId, image);
            return Ok("success");
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteProfilePicture([FromRoute] string userId)
        {
            await userProfileImageService.DeleteProfilePictureAsync(userId);
            return Ok("success");
        }
    }
}
