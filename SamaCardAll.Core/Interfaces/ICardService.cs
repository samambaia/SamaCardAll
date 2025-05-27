using SamaCardAll.Infra.Models;

namespace SamaCardAll.Core.Interfaces
{
    public interface ICardService
    {
        Task<IEnumerable<Card>> GetCardsAsync();
        Task<IEnumerable<Card>> GetActiveCardsAsync();
        Task<Card> GetByIdAsync(int id);
        Task CreateAsync(Card card);
        Task UpdateAsync(Card card);
        Task DeleteAsync(int id);
    }
}
