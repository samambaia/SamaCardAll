using SamaCardAll.Infra;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Core.Services
{
    public class CardService : ICardService
    {

        private readonly AppDbContext _context;
        private readonly List<Card> _cards;

        public CardService(AppDbContext context)
        {
            _context = context;

            _cards = _context.Cards.ToList();
        }

        public IEnumerable<Card> GetCards()
        {
            return _cards;
        }

        public Card GetById(int id)
        {
            return _cards.FirstOrDefault(s => s.IdCard == id);
        }

        public void Create(Card card)
        {

            card.IdCard = _cards.Count + 1;

            _context.Add(card);
            _context.SaveChanges();
        }

        public void Update(Card card)
        {
            var existingCard = _cards.FirstOrDefault(s => s.IdCard == card.IdCard);

            if (existingCard != null)
            {
                existingCard.Bank = card.Bank;
                existingCard.Brand = card.Brand;
                existingCard.Number = card.Number;
                existingCard.Expiration = card.Expiration;
                existingCard.Active = card.Active;

                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var cardToRemove = _cards.FirstOrDefault(s => s.IdCard == id);

            if (cardToRemove != null)
            {
                _context.Remove(cardToRemove);
                _context.SaveChanges();
            }
        }
    }
}
