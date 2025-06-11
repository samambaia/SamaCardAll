using SamaCardAll.Core.VO;
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

        public async Task<bool> DeleteAsync(int id)
        {
            return await cardRepository.DeleteAsync(id);
        }

        public async Task<List<CardVO>> GetActiveCardsAsync()
        {
            return await cardRepository.GetActiveCardsAsync();
        }

        public async Task<CardVO> GetByIdAsync(int id)
        {
            return await cardRepository.GetByIdAsync(id);
        }

        public async Task<List<CardVO>> GetCardsAsync()
        {
            return await cardRepository.GetCardsAsync();
        }

        public async Task<bool> UpdateAsync(CardVO card)
        {
            return await cardRepository.UpdateAsync(card);
        }
    }
}
