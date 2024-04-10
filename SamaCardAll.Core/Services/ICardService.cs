using SamaCardAll.Infra.Models;

namespace SamaCardAll.Core.Services
{
    public interface ICardService
    {
        IEnumerable<Card> GetCards();
        IEnumerable<Card> GetActiveCards();
        Card GetById(int id);
        void Create(Card card);
        void Update(Card card);
        void Delete(int id);

    }
}
