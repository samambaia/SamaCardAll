using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.Models;
using SamaCardAll.Shared.Contracts.DTOs;

namespace SamaCardAll.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IAuthRepository _authRepo;

        public UserService(IUserRepository userRepo, IAuthRepository authRepo)
        {
            _userRepo = userRepo;
            _authRepo = authRepo;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            // 1. Obter o usuário pelo ID
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
            {
                // O usuário não deve ser nulo se o AuthContextService estiver correto,
                // mas é um bom check de segurança.
                return false;
            }

            // 2. Verificar a Senha Atual
            // Seu repositório precisa de um método para atualizar o usuário.
            // Assumo que a classe User tem a propriedade PasswordHash.
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash);

            if (!isPasswordValid)
            {
                return false; // Senha atual incorreta
            }

            // 3. Gerar novo Hash e Atualizar
            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword, workFactor: 12);

            user.PasswordHash = newPasswordHash;

            // 🚨 Assumindo que IUserRepository tem um método Update
            await _userRepo.UpdateAsync(user);

            return true;
        }

        public async Task<UserResponseDto> RegisterUserAsync(RegisterUserDto dto)
        {
            var existing = await _userRepo.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new Exception("User already exists");

            var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password, workFactor: 12);

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = hash,
                FullName = dto.FullName
            };

            user.Id = await _userRepo.CreateAsync(user);

            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName
            };
        }


        public async Task<bool> SetUserActiveStatusAsync(int userId, bool isActive)
        {
            var user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
                return false;

            bool statusChangedToInactive = user.IsActive != isActive && isActive == false;

            user.IsActive = isActive;
            await _userRepo.UpdateAsync(user);

            if (statusChangedToInactive)
            {
                await _authRepo.RemoveRefreshTokenAsync(userId);
            }

            return true;
        }

        public async Task<bool> ResetPasswordByAdminAsync(int userId, string newPassword)
        {
            var user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
                return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword, workFactor: 12);

            user.IsActive = true; // Reativar usuário se estava inativo

            await _userRepo.UpdateAsync(user);

            return true;
        }
    }
}