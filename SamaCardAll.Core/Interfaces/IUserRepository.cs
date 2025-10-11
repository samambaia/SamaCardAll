using SamaCardAll.Core.Models;

namespace SamaCardAll.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<long> CreateAsync(User user);  
        Task<User?> GetByIdAsync(long id);
    }
}