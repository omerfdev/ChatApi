using Application.Services.UserService.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserService.Implementations
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public int GetAuthenticatedUserId()
        {
            var userId = 0;
            if (httpContextAccessor.HttpContext != null)
            {
                var NameIdentifier = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                userId = int.Parse(NameIdentifier);
            }
            return userId;
        }

        public string GetAuthenticatedUsername()
        {
            var username = string.Empty;
            if (httpContextAccessor.HttpContext != null)
            {
                username = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            }
            return username;
        }
    }
}
