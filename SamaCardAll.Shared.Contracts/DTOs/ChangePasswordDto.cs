using System.ComponentModel.DataAnnotations;

namespace SamaCardAll.Shared.Contracts.DTOs
{
    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "The new password must be at least 6 characters long.")]
        public string NewPassword { get; set; } = string.Empty;
    }
}