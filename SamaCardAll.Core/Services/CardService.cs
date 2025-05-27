using SamaCardAll.Core.Interfaces;
using SamaCardAll.Infra;
using SamaCardAll.Infra.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SamaCardAll.Core.Services
{
    public class CardService : ICardService
    {
        private readonly AppDbContext _context;

        public CardService(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task UpdateAsync(Card card)
        {
            var existingCard = await _context.Cards.FirstOrDefaultAsync(s => s.IdCard == card.IdCard);

            if (existingCard != null)
            {
                existingCard.Bank = card.Bank;
                existingCard.Brand = card.Brand;
                existingCard.Number = card.Number;
                existingCard.Expiration = card.Expiration;
                existingCard.Active = card.Active;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var cardToRemove = await _context.Cards.FirstOrDefaultAsync(s => s.IdCard == id);

            if (cardToRemove != null)
            {
                _context.Cards.Remove(cardToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Card>> GetActiveCardsAsync()
        {
            return await _context.Cards.Where(c => c.Active == 1).ToListAsync();
        }

        public async Task<IEnumerable<Card>> GetCardsAsync()
        {
            return await _context.Cards.ToListAsync();
        }

        public async Task<Card> GetByIdAsync(int id)
        {
            return await _context.Cards.FirstOrDefaultAsync(s => s.IdCard == id)
                ?? throw new InvalidOperationException($"Card with ID {id} not found.");
        }

        public async Task CreateAsync(Card card)
        {
            card.IdCard = await _context.Cards.MaxAsync(s => s.IdCard) + 1;
            await _context.AddAsync(card);
            await _context.SaveChangesAsync();
        }
    }
}
