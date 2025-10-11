using Microsoft.AspNetCore.Mvc;
using SamaCardAll.Core.Services;
using SamaCardAll.Shared.Contracts.DTOs;

namespace SamaCardAll.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        public UsersController(UserService userService) => _userService = userService;

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
    }
}