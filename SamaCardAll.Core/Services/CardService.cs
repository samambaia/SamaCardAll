using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.Models;

namespace SamaCardAll.Core.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository cardRepository;

        public CardService(ICardRepository repository)
        {
            cardRepository = repository;
        }

        public async Task CreateAsync(Card card)
        {
            await cardRepository.CreateAsync(card);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await cardRepository.DeleteAsync(id);
        }

        public async Task<List<Card>> GetActiveCardsAsync()
        {
            return await cardRepository.GetActiveCardsAsync();
        }

        public async Task<Card> GetByIdAsync(int id)
        {
            return await cardRepository.GetByIdAsync(id);
        }

        public async Task<List<Card>> GetCardsAsync()
        {
            return await cardRepository.GetCardsAsync();
        }

        public async Task<bool> UpdateAsync(Card card)
        {
            return await cardRepository.UpdateAsync(card);
        }
    }
}
