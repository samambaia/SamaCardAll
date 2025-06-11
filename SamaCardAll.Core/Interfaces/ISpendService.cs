using SamaCardAll.Core.VO;

namespace SamaCardAll.Core.Interfaces
{
    public interface ISpendService
    {
        Task<List<SpendVO>> GetSpendsAsync();
        Task<SpendVO> GetByIdAsync(int id);
        Task CreateAsync(SpendVO spend);
        Task<bool> UpdateAsync(SpendVO spend);
        Task<bool> DeleteAsync(int id);
    }
}
