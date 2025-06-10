using SamaCardAll.Core.VO;

namespace SamaCardAll.Core.Interfaces
{
    public interface ISpendRepository
    {
        Task<List<SpendVO>> GetSpendsAsync();
        Task<SpendVO> GetByIdAsync(int id);
        Task CreateAsync(SpendVO spend);
        Task UpdateAsync(SpendVO spend);
        Task DeleteAsync(int id);
    }
}
