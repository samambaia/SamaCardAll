using System.Text.Json.Serialization;

namespace FrontMaui.Models;

public class Card
{
    [JsonPropertyName("idCard")]
    public int IdCard { get; set; }
    [JsonPropertyName("bank")]
    public string? Bank { get; set; }
    [JsonPropertyName("number")]
    public string? Number { get; set; }
    [JsonPropertyName("expiration")]
    public string? Expiration { get; set; }
    [JsonPropertyName("brand")]
    public string? Brand { get; set; }
    [JsonPropertyName("active")]
    public short Active { get; set; } 
}
