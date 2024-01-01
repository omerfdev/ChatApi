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
            string userId = null; // Default olarak null olarak başlatıldı.

            if (httpContextAccessor.HttpContext != null)
            {
                var nameIdentifier = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (nameIdentifier != null && int.TryParse(nameIdentifier, out int parsedUserId))
                {
                    userId = parsedUserId.ToString(); // int'i string'e çevirerek atama yapılıyor.
                }
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
