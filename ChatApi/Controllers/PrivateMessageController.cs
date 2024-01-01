using Application.Services.PrivateMessageServices.Implementations;
using Application.Services.PrivateMessageServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers
{
    [Route("api/private-messages")]
    [ApiController]
    public class PrivateMessagesController : ControllerBase
    {
        private readonly IPrivateMessageService privateMessageService;

        public PrivateMessagesController(IPrivateMessageService privateMessageService)
        {
            this.privateMessageService = privateMessageService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPrivateMessages(
            DateTime? pageDate,
            int pageSize,
            string firstUserId,
            string secoundUserId)
        {
            var result = await privateMessageService.GetPrivateMessages(pageDate, pageSize, firstUserId, secoundUserId);
            return Ok(result);
        }
    }
}
