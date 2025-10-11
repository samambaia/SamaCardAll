using System.ComponentModel.DataAnnotations;

namespace SamaCardAll.Core.Models
{
    public class RefreshToken
    {
        [Key]
        public string Token { get; set; } = string.Empty;
        public int UserIdUser { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => RevokedAt == null && !IsExpired;
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    }
}