namespace SamaCardAll.Core.Entities
{
    public class RefreshToken
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }
        public string? ReplacedByToken { get; set; }
        public bool IsActive => RevokedAt == null && !IsExpired;
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    }
}
