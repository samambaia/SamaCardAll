using SamaCardAll.Core.VO;

namespace SamaCardAll.Core.Interfaces
{
    public interface ICardRepository
    {
        Task<List<CardVO>> GetCardsAsync();
        Task<List<CardVO>> GetActiveCardsAsync();
        Task<CardVO> GetByIdAsync(int id);
        Task CreateAsync(CardVO card);
        Task<bool> UpdateAsync(CardVO card);
        Task<bool> DeleteAsync(int id);
    }
}
