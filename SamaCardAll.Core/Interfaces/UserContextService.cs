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
            // Tenta obter o ID a partir do Claim 'NameIdentifier' (padrão)
            var userIdClaim = _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }

            // Isso indica que o token é inválido ou está faltando o claim do ID
            throw new InvalidOperationException("Usuário não autenticado ou Claim de ID ausente.");
        }
    }
}
