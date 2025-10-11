namespace SamaCardAll.Core.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string? AccessToken, string? RefreshToken, string? Message)> AuthenticateAsync(string email, string password);
        Task<(bool Success, string? AccessToken, string? RefreshToken, string? Message)> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);
    }
}