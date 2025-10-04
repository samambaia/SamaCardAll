using System.Text.Json.Serialization;

namespace FrontMaui.Models;

public class Customer
{
    [JsonPropertyName("idCustomer")]
    public int IdCustomer { get; set; }
    [JsonPropertyName("customerName")]  
    public string? CustomerName { get; set; }
    [JsonPropertyName("active")]        
    public short Active { get; set; }
}
