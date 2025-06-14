using System.Text.Json.Serialization;

namespace FrontWeb.DTO
{
    public class SpendDTO
    {
        public SpendDTO()
        {
            Expenses = string.Empty;
            Amount = 0.0m;
            Date = DateTime.Now;
            InstallmentPlan = 0;
            InstallmentValue = 0.0m;
            Deleted = 0;
            CreatedDate = DateTime.Now;
            Customer = new CustomerDTO();
            Card = new CardDTO();
            User = new UserDTO();
        }

        [JsonPropertyName("idSpend")]
        public int IdSpend { get; set; }

        [JsonPropertyName("expenses")]
        public string Expenses { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("installmentPlan")]
        public int InstallmentPlan { get; set; }

        [JsonPropertyName("installmentValue")]
        public decimal InstallmentValue { get; set; }

        [JsonPropertyName("deleted")]
        public int Deleted { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("customerIdCustomer")]
        public int CustomerIdCustomer { get; set; }

        [JsonPropertyName("customer")]
        public CustomerDTO Customer { get; set; }

        [JsonPropertyName("cardIdCard")]
        public int CardIdCard { get; set; }

        [JsonPropertyName("card")]
        public CardDTO Card { get; set; }

        [JsonPropertyName("userIdUser")]
        public int UserIdUser { get; set; }

        [JsonPropertyName("user")]
        public UserDTO User { get; set; }
    }
}