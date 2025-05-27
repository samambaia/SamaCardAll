using SamaCardAll.Infra.Models;

namespace SamaCardAll.Core.Interfaces
{
    public interface ISpendService
    {
        Task<IEnumerable<Spend>> GetSpendsAsync();
        Task<Spend> GetByIdAsync(int id);
        Task CreateAsync(Spend spend);
        Task UpdateAsync(Spend spend);
        Task DeleteAsync(int id);
    }
}
