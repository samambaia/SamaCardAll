using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.Models;

namespace SamaCardAll.Infra.Repository
{
    public class CardRepository : ICardRepository
    {
        private readonly IUserContextService _userContext;
        private readonly AppDbContext _context;
        private readonly int _userId;

        public CardRepository(AppDbContext context, IUserContextService userContext)
        {
            _context = context; 
            _userContext = userContext;
            _userId = _userContext.GetUserId();
        }

        public async Task CreateAsync(Card card)
        {
            card.UserIdUser = _userId;

            // Add and save the new card
            await _context.Cards.AddAsync(card);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cardToRemove = await GetByIdAsync(id);

            if (cardToRemove == null)
                return false;

            _context.Cards.Remove(cardToRemove);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Card>> GetActiveCardsAsync()
        {
            var activeCards = await _context.Cards.Where(c => c.Active == 1 && c.UserIdUser == _userId).ToListAsync();

            return [.. activeCards.Select(c => new Card
            {
                IdCard = c.IdCard,
                Bank = c.Bank,
                Number = c.Number,
                Expiration = c.Expiration,
                Brand = c.Brand,
                Active = c.Active
            })];
        }

        public async Task<Card> GetByIdAsync(int id)
        {
            var card = await _context.Cards
                .Where(c => c.UserIdUser == _userId)
                .FirstOrDefaultAsync(c => c.IdCard == id);

            return card;
        }

        public async Task<List<Card>> GetCardsAsync()
        {
            var cards = await _context.Cards
                .Where(c => c.UserIdUser == _userId)
                .ToListAsync();
            return cards;
        }

        public async Task<bool> UpdateAsync(Card card)
        {
            var existingCard = await GetByIdAsync(card.IdCard);

            if (existingCard == null)
                return false;

            existingCard.Bank = card.Bank;
            existingCard.Number = card.Number;
            existingCard.Expiration = card.Expiration;
            existingCard.Brand = card.Brand;
            existingCard.Active = card.Active;
            existingCard.UserIdUser = _userId;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
