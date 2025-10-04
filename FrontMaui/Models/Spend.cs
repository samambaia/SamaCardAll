using System.Text.Json.Serialization;

namespace FrontMaui.Models;

public class Spend
{
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
    public short Deleted { get; set; }
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    [JsonPropertyName("customerIdCustomer")]
    public int CustomerIdCustomer { get; set; }
    [JsonPropertyName("customerName")]
    public string CustomerName { get; set; }
    [JsonPropertyName("cardIdCard")]
    public int CardIdCard { get; set; }
    [JsonPropertyName("cardName")]
    public string CardName { get; set; }
    [JsonPropertyName("userIdUser")]
    public int UserIdUser { get; set; }
    [JsonPropertyName("userName")]
    public string UserName { get; set; }
}
