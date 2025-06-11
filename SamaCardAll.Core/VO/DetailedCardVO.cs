namespace SamaCardAll.Core.VO
{
    public record DetailedCardVO
    {
        public int IdCard { get; init; }
        public string CardName { get; init; }
        public string DescriptionSpend { get; init; }
        public string CustomerName { get; init; }
        public decimal InstallmentAmount { get; init; }
        public string MonthYear { get; init; }
        public string Installment { get; init; }
        public DetailedCardVO(int idCard, string cardName, string descriptionSpend, string customerName, decimal installmentAmount, string monthYear, string installment)
        {
            IdCard = idCard;
            CardName = cardName;
            DescriptionSpend = descriptionSpend;
            CustomerName = customerName;
            InstallmentAmount = installmentAmount;
            MonthYear = monthYear;
            Installment = installment;
        }
    }
}