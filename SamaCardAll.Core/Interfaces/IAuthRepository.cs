using SamaCardAll.Core.Models;

namespace SamaCardAll.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task AddRefreshTokenAsync(RefreshToken token);
        Task RevokeRefreshTokenAsync(string token, string? replacedByToken = null);
        Task RemoveRefreshTokenAsync(int userId);
    }
}