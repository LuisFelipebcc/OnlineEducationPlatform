using System.Threading.Tasks;
using IdentityService.Domain.DTOs;

namespace IdentityService.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<bool> ValidateTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<bool> UpdateUserAsync(string userId, UserDto userDto);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> ResetPasswordAsync(string email);
        Task<bool> ConfirmEmailAsync(string userId, string token);
    }
}