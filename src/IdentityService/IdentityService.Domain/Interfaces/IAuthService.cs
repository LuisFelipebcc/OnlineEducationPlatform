using System.Threading.Tasks;
using IdentityService.Domain.DTOs;

namespace IdentityService.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDTO> LoginAsync(LoginDTO loginDTO);
        Task<UserDTO> RegisterAsync(RegisterDTO registerDTO);
        Task<bool> ValidateTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
        Task<UserDTO> GetUserByIdAsync(string userId);
        Task<bool> UpdateUserAsync(string userId, UserDTO userDTO);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> ResetPasswordAsync(string email);
        Task<bool> ConfirmEmailAsync(string userId, string token);
    }
}