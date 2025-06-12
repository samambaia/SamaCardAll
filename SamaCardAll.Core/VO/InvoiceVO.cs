namespace SamaCardAll.Core.VO
{
    public record InvoiceVO
    {
        public string DescriptionSpend { get; init; }
        public string CustomerName { get; init; }
        public string CardName { get; init; }
        public decimal InstallmentAmount { get; init; }
        public string MonthYear { get; init; }
        public string Installment { get; init; }

        public InvoiceVO(string descriptionSpend, string customerName, string cardName, decimal installmentAmount, string monthYear, string installment)
        {
            DescriptionSpend = descriptionSpend;
            CustomerName = customerName;
            CardName = cardName;
            InstallmentAmount = installmentAmount;
            MonthYear = monthYear;
            Installment = installment;
        }
    }
}
