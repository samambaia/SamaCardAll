namespace SamaCardAll.Core.DTO
{
    public record InvoiceDto(int IdInvoice, int IdCard, string MonthYear, decimal TotalAmount, decimal TotalPaid, decimal TotalPending) : IEquatable<InvoiceDto>
    {
        public int IdInvoice { get; set; } = IdInvoice;
        public int IdCard { get; set; } = IdCard;
        public string MonthYear { get; set; } = MonthYear;
        public decimal TotalAmount { get; set; } = TotalAmount;
        public decimal TotalPaid { get; set; } = TotalPaid;
        public decimal TotalPending { get; set; } = TotalPending;
    }
}
