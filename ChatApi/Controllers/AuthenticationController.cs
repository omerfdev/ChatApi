using Application.Models.DTOs.UserDTOs;
using Application.Services.UserService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserAccountService userAccountService;
        public AuthenticationController(IUserAccountService userAccountService)
        {
            this.userAccountService = userAccountService;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Register(UserRequestDto userRegistration)
        {
            await userAccountService.RegisterUserAsync(userRegistration);
            return Ok("success");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserRequestDto userLogin)
        {
            var token = await userAccountService.LoginUserAsync(userLogin);
            return Ok(token);
        }

        [Authorize]
        [HttpGet("identification")]
        public async Task<IActionResult> Identification()
        {
            var user = await userAccountService.GetUserByJwtTokenAsync();
            return Ok(user);
        }
    }
}
