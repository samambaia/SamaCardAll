using SamaCardAll.Core.Models;

namespace SamaCardAll.Core.Interfaces
{
    public interface ICardRepository
    {
        Task<List<Card>> GetCardsAsync();
        Task<List<Card>> GetActiveCardsAsync();
        Task<Card> GetByIdAsync(int id);
        Task CreateAsync(Card card);
        Task<bool> UpdateAsync(Card card);
        Task<bool> DeleteAsync(int id);
    }
}
