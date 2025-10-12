using SamaCardAll.Shared.Contracts.DTOs;

namespace SamaCardAll.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> RegisterUserAsync(RegisterUserDto dto);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    }
}