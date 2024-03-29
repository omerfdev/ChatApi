﻿using Application.Services.PrivateMessageServices.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers
{
    [Route("api/users/{userId}/recent-chats")]
    [ApiController]
    public class RecentChatsController : ControllerBase
    {
        private readonly IPrivateMessageService privateMessageService;

        public RecentChatsController(IPrivateMessageService privateMessageService)
        {
            this.privateMessageService = privateMessageService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetRecentChatsForUser(int userId)
        {
            var result = await privateMessageService.GetRecentChatsForUser(userId);
            return Ok(result);
        }
    }
}
