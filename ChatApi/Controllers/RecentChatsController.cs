using Application.Services.PrivateMessageServices.Implementations;
using Application.Services.PrivateMessageServices.Interfaces;
using Application.Services.UserService.Interfaces;
using BusinessLayer.Services.UserService.Implementations;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApi.Controllers
{
    [Route("api/users/{userId}/recent-chats")]
    [ApiController]
    public class RecentChatsController : ControllerBase
    {
        private readonly IPrivateMessageService privateMessageService;
        private readonly IUserAccountService userAccountService;

        public RecentChatsController(IUserAccountService userAccountService, IPrivateMessageService privateMessageService)
        {
            this.userAccountService = userAccountService;
            this.privateMessageService = privateMessageService;
        }      

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetRecentChatsForUser(string userId)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != currentUserId)
            {
                return Forbid(); // Yetkilendirme hatası
            }

            var result = await privateMessageService.GetRecentChatsForUser(userId);
            return Ok(result);
        }
    }
}
