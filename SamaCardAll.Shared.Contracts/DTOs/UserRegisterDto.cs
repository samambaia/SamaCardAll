// SamaCardAll.Shared.Contracts.DTOs/RegisterUserDto.cs
using System.ComponentModel.DataAnnotations;

namespace SamaCardAll.Shared.Contracts.DTOs
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O formato do e-mail é inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 255 caracteres.")]
        public string Password { get; set; } = string.Empty;
    }
}