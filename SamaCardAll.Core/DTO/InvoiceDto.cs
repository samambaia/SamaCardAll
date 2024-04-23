namespace SamaCardAll.Core.DTO
{
    public class InvoiceDto
    {
        public string DescriptionSpend { get; set; }
        public string CustomerName { get; set; }
        public string CardName { get; set; }
        public decimal InstallmentAmount { get; set; }
        public string MonthYear { get; set; }
    }
}
