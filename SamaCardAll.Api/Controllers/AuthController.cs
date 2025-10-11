using Microsoft.AspNetCore.Mvc;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Shared.Contracts.DTOs;

namespace SamaCardAll.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;     
        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserRegisterDto dto)
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
    }
}
