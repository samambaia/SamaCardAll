using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace SamaCardAll.Core.Interfaces
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null || httpContext.User == null || !httpContext.User.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated or invalid token.");
            }

            var userIdClaim = httpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }

            throw new InvalidOperationException("Missing claim ID from valid token.");
        }
    }
}
