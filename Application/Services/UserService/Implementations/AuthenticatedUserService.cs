using Application.Services.UserService.Interfaces;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System.Security.Claims;

namespace BusinessLayer.Services.UserService.Implementations
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetAuthenticatedUserId()
        {
            var userId = "0";
            if (httpContextAccessor.HttpContext != null)
            {
                var NameIdentifier = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                userId = NameIdentifier;
            }
            return userId;
        }


        public string GetAuthenticatedUsername()
        {
            var username = string.Empty;
            if (httpContextAccessor.HttpContext != null)
            {
                username = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            }
            return username;
        }
    }
}
