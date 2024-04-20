namespace FrontWeb
{
    public class InvoiceDto
    {
        public string CustomerName { get; set; }
        public string CardName { get; set; }
        public decimal InstallmentAmount { get; set; }
        public string MonthYear { get; set; }
    }
}
