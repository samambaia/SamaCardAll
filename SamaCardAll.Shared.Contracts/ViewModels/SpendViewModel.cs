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
        public CustomerViewModel Customer { get; set; } = new CustomerViewModel();

        public int CardIdCard { get; set; }
        public CardViewModel Card { get; set; } = new CardViewModel();

        public int UserIdUser { get; set; }
        public UserViewModel User { get; set; } = new UserViewModel();

        public SpendViewModel() 
        {
            Customer = new CustomerViewModel();
            Card = new CardViewModel();
            User = new UserViewModel();
        } // Construtor padrão para o Blazor
    }
}