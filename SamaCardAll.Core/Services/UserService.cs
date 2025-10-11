using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.Models;
using SamaCardAll.Shared.Contracts.DTOs;

namespace SamaCardAll.Core.Services
{
    public class UserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo) => _repo = repo;

        public async Task<UserResponseDto> RegisterUserAsync(UserRegisterDto dto)
        {
            var existing = await _repo.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new Exception("User already exists");

            var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password, workFactor: 12);

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = hash,
                FullName = dto.FullName
            };

            user.Id = await _repo.CreateAsync(user);

            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName
            };
        }
    }
}