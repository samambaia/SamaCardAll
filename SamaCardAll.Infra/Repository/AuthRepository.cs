using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.Models;

namespace SamaCardAll.Infra.Repository
{
    public class AuthRepository(AppDbContext context) : IAuthRepository
    {
        private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task AddRefreshTokenAsync(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string token)
        {
            return await _context.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task RemoveRefreshTokenAsync(int userId)
        {
            await _context.RefreshTokens
                .Where(t => t.UserIdUser == userId)
                .ExecuteDeleteAsync();
        }

        public async Task RevokeRefreshTokenAsync(string token, string replacedByToken = null)
        {
            var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == token);

            if (existingToken != null)
            {
                existingToken.RevokedAt = DateTime.UtcNow;
                existingToken.ReplacedByToken = replacedByToken;
                await _context.SaveChangesAsync();
            }
        }
    }
}
