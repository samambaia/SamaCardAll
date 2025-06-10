using SamaCardAll.Core.DTO;
using SamaCardAll.Core.Interfaces;

namespace SamaCardAll.Core.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository cardRepository;

        public CardService(ICardRepository repository)
        {
            cardRepository = repository;
        }

        public async Task CreateAsync(CardVO card)
        {
            await cardRepository.CreateAsync(card);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var result = cardRepository.DeleteAsync(id);

            return result ?? throw new ArgumentNullException(nameof(id), "Delete operation returned null.");
        }

        public async Task<List<CardVO>> GetActiveCardsAsync()
        {
            var activeCards = await cardRepository.GetActiveCardsAsync();
            return activeCards ?? throw new InvalidOperationException("No active cards found.");
        }

        public async Task<CardVO> GetByIdAsync(int id)
        {
            var card = await cardRepository.GetByIdAsync(id);
            return card ?? throw new InvalidOperationException($"Card with ID {id} not found.");
        }

        public async Task<List<CardVO>> GetCardsAsync()
        {
            var cards = await cardRepository.GetCardsAsync();
            return cards ?? throw new InvalidOperationException("No cards found.");
        }

        public async Task<bool> UpdateAsync(CardVO card)
        {
            bool result = await cardRepository.UpdateAsync(card);

            if (!result)
                throw new ArgumentNullException(nameof(card), "Card cannot be updated.");
            else
                return result;

        }
    }
}
