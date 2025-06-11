using SamaCardAll.Core.VO;

namespace SamaCardAll.Infra.Mapping
{
    public static class CardMappingExtensions
    {
        public static CardVO ToVO(this Models.Card card)
        {
            if (card == null) return null;
            return new Core.VO.CardVO(
                IdCard: card.IdCard,
                Bank: card.Bank,
                Number: card.Number,
                Expiration: card.Expiration,
                Brand: card.Brand,
                Active: card.Active
            );
        }
        public static Models.Card ToModel(this Core.VO.CardVO cardVO)
        {
            if (cardVO == null) return null;
            return new Models.Card
            {
                IdCard = cardVO.IdCard,
                Bank = cardVO.Bank,
                Number = cardVO.Number,
                Expiration = cardVO.Expiration,
                Brand = cardVO.Brand,
                Active = cardVO.Active
            };
        }
    }
}
