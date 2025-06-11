using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.VO;
using SamaCardAll.Infra.Mapping;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Infra.Repository
{
    public class CardRepository(AppDbContext context) : ICardRepository
    {
        private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task CreateAsync(CardVO cardVO)
        {
            var card = new Card
            {
                IdCard = await _context.Cards.MaxAsync(s => s.IdCard) + 1,
                Bank = cardVO.Bank,
                Number = cardVO.Number,
                Expiration = cardVO.Expiration,
                Brand = cardVO.Brand,
                Active = cardVO.Active
            };
            await _context.AddAsync(card);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cardToRemove = await _context.Cards.FindAsync(id);

            if (cardToRemove == null)
                return false;

            _context.Cards.Remove(cardToRemove);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CardVO>> GetActiveCardsAsync()
        {
            var activeCards = await _context.Cards.Where(c => c.Active == 1).ToListAsync();

            return activeCards.Select(c => new CardVO(IdCard: c.IdCard, Bank: c.Bank, Number: c.Number, Expiration: c.Expiration, Brand: c.Brand, Active: c.Active)).ToList();
        }

        public async Task<CardVO> GetByIdAsync(int id)
        {
            var getCard = await _context.Cards.FindAsync(id);
            return getCard.ToVO();

        }

        public async Task<List<CardVO>> GetCardsAsync()
        {
            var getCards = await _context.Cards.ToListAsync();

            return [.. getCards.Select(c => c.ToVO())];
        }

        public async Task<bool> UpdateAsync(CardVO card)
        {
            var existingCard = await _context.Cards.FindAsync(card.IdCard);

            if (existingCard != null)
            {
                existingCard.Bank = card.Bank;
                existingCard.Number = card.Number;
                existingCard.Expiration = card.Expiration;
                existingCard.Brand = card.Brand;
                existingCard.Active = card.Active;
                await _context.SaveChangesAsync();

                return true;

            }
            else
                return false;
        }
    }
}
