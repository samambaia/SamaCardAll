using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Shared.Contracts.DTOs;

namespace SamaCardAll.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IUserContextService _userContextService;

        public AuthController(IAuthService authService, IUserService userService, IUserContextService userContextService) =>
            (_authService, _userService, _userContextService) = (authService, userService, userContextService);

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)] // Para e-mail já existente
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Chamamos seu método existente
                var newUser = await _userService.RegisterUserAsync(dto);

                // Sucesso no cadastro
                // Você pode retornar o DTO do novo usuário, se quiser
                return CreatedAtAction(nameof(Register), newUser);
            }
            catch (Exception ex) when (ex.Message.Contains("User already exists"))
            {
                // 🚨 Tratamento da exceção específica do seu UserService
                return Conflict(ex.Message); // 409 Conflict
            }
            catch (Exception ex)
            {
                // Erro de servidor inesperado
                // O ideal é logar a exceção completa aqui
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal error while registering user.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.AuthenticateAsync(dto.Email, dto.Password);
            if (!result.Success)
                return Unauthorized(new { message = result.Message });

            return Ok(new { accessToken = result.AccessToken, refreshToken = result.RefreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto.RefreshToken);
            if (!result.Success)
                return Unauthorized(new { message = result.Message });

            return Ok(new { accessToken = result.AccessToken, refreshToken = result.RefreshToken });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDto dto)
        {
            var revoked = await _authService.RevokeTokenAsync(dto.RefreshToken);
            return revoked ? NoContent() : NotFound();
        }

        [Authorize] // 🚨 Garante que só usuários logados podem acessar
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            // A validação do DTO (via [Required] e [MinLength]) é feita automaticamente.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 1. Obtém o ID do usuário do token JWT
            int userId = _userContextService.GetUserId();

            if (userId == 0)
            {
                // Falha na extração do ID (não deve ocorrer se [Authorize] estiver OK)
                return Unauthorized(new { message = "User not authenticated." });
            }

            // 2. Verifica se as senhas nova e atual são as mesmas
            if (dto.CurrentPassword == dto.NewPassword)
            {
                return BadRequest(new { message = "The new password must be different from the current password." });
            }

            // 3. Chama a lógica de serviço
            bool success = await _userService.ChangePasswordAsync(
                userId,
                dto.CurrentPassword,
                dto.NewPassword
            );

            if (success)
            {
                return Ok(new { message = "Password changed successfully!" });
            }

            // 4. Se falhou (geralmente senha atual incorreta)
            return BadRequest(new { message = "Unable to change the password. Please check if the current password is correct." });
        }
    }
}
