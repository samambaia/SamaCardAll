using SamaCardAll.Core.Models;

namespace SamaCardAll.Core.Interfaces
{
    public interface ISpendService
    {
        Task<List<Spend>> GetSpendsAsync();
        Task<Spend> GetByIdAsync(int id);
        Task CreateAsync(Spend spend);
        Task<bool> UpdateAsync(Spend spend);
        Task<bool> DeleteAsync(int id);
    }
}
