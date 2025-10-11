public class TokenResponse
{
    // Ajuste estes nomes para corresponder à sua API
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expiration { get; set; }
}