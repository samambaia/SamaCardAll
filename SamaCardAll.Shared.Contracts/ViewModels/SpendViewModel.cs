namespace SamaCardAll.Shared.Contracts.ViewModels
{
    public class SpendViewModel
    {
        // Propriedades mutáveis para ligação de dados no Blazor
        public int IdSpend { get; set; }
        public string Expenses { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int InstallmentPlan { get; set; } = 1;
        public decimal InstallmentValue { get; set; } = 0.0m;
        public short Deleted { get; set; } = 0; // 0 = Não deletado, 1 = Deletado
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int CustomerIdCustomer { get; set; }
        public string CustomerName { get; set; }
        public int CardIdCard { get; set; }
        public string CardName { get; set; }
        public int UserIdUser { get; set; }
        public string UserName { get; set; }
    }
}