using SamaCardAll.Core.Models;

namespace SamaCardAll.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<int> CreateAsync(User user);  
        Task<User?> GetByIdAsync(int id);
        Task UpdateAsync(User user);
    }
}