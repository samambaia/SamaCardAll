using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SamaCardAll.Api.DTO;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Shared.Contracts.DTOs;
using System.Security.Claims;

namespace SamaCardAll.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) => _userService = userService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            try
            {
                var result = await _userService.RegisterUserAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{userId}/change-status")]
        public async Task<IActionResult> ChangeStatus(int userId, [FromBody] UserStatusUpdateDto dto)
        {
            var success = await _userService.SetUserActiveStatusAsync(userId, dto.IsActive);

            if (!success)
                return NotFound(new { error = "User not found" });

            return Ok(new { Message = $"User {userId} {(dto.IsActive ? "Activated" : "Deactivated")}"});
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{userId}/reset-password")]
        public async Task<IActionResult> ResetPassword(int userId, [FromBody] AdminPasswordResetDto dto)
        {
            if (userId.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid("Admins cannot reset their own password via this endpoint.");
            }

            var success = await _userService.ResetPasswordByAdminAsync(userId, dto.NewPassword);
            if (!success)
                return NotFound(new { error = "User not found" });

            return Ok(new { Message = "Password reset successfully" });
        }
    }
}