namespace SamaCardAll.Core.VO
{
    public record InvoiceVO
    {
        //public int IdInvoice { get; init; }
        public int IdCard { get; init; }
        public string MonthYear { get; init; }
        public decimal TotalAmount { get; init; }
        public decimal TotalPaid { get; init; }
        public decimal TotalPending { get; init; }

        public InvoiceVO(int idCard, string monthYear, decimal totalAmount, decimal totalPaid, decimal totalPending)
        {
            IdCard = idCard;
            MonthYear = monthYear;
            TotalAmount = totalAmount;
            TotalPaid = totalPaid;
            TotalPending = totalPending;
        }
    }
}
