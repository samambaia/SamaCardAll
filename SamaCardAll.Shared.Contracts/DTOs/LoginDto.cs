using System.ComponentModel.DataAnnotations;

namespace SamaCardAll.Shared.Contracts.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "The e-email is required.")]
        [EmailAddress(ErrorMessage = "The email format is invalid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "The password is required.")]
        public string Password { get; set; } = string.Empty;
    }
}