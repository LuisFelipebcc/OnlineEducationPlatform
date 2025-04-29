using System.Threading.Tasks;
using IdentityService.Application.DTOs;

namespace IdentityService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDTO> LoginAsync(LoginDTO loginDTO);
        Task<UsuarioDTO> RegistrarAsync(RegistroDTO registroDTO);
        Task<TokenDTO> RefreshTokenAsync(string refreshToken);
        Task<bool> ValidarTokenAsync(string token);
    }
}