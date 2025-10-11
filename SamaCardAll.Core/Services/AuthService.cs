using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SamaCardAll.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IAuthRepository _authRepo;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepo, IAuthRepository authRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _authRepo = authRepo;
            _config = config;
        }

        public async Task<(bool Success, string AccessToken, string RefreshToken, string Message)> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepo.GetByEmailAsync(email);
            if (user == null)
                return (false, null, null, "User not found");

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return (false, null, null, "Invalid credentials");

            var jwt = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(user.Id);
            await _authRepo.AddRefreshTokenAsync(refreshToken);

            return (true, jwt, refreshToken.Token, null);
        }

        public async Task<(bool Success, string AccessToken, string RefreshToken, string Message)> RefreshTokenAsync(string refreshToken)
        {
            var stored = await _authRepo.GetRefreshTokenAsync(refreshToken);
            if (stored == null || !stored.IsActive)
                return (false, null, null, "Invalid refresh token");

            var user = await _userRepo.GetByIdAsync(stored.UserIdUser);
            if (user == null)
                return (false, null, null, "User not found");

            var newJwt = GenerateJwtToken(user);
            var newRefresh = GenerateRefreshToken(user.Id);

            await _authRepo.RevokeRefreshTokenAsync(refreshToken, newRefresh.Token);
            await _authRepo.AddRefreshTokenAsync(newRefresh);

            return (true, newJwt, newRefresh.Token, null);
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            var stored = await _authRepo.GetRefreshTokenAsync(refreshToken);
            if (stored == null || !stored.IsActive) return false;
            await _authRepo.RevokeRefreshTokenAsync(refreshToken);
            return true;
        }

        private string GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("FullName", user.FullName)
            }), //TODO
                Expires = DateTime.UtcNow.AddSeconds(30), //DateTime.UtcNow.AddMinutes(15), //Change when go to production
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(int userId)
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return new RefreshToken
            {
                UserIdUser = userId,
                Token = Convert.ToBase64String(randomBytes),
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };
        }
    }
}
