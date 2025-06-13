using IdentityService.Application.DTOs;

namespace IdentityService.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UsuarioDto?> RegistrarAsync(RegistrarUsuarioDto registrarUsuarioDto);
        Task<UsuarioDto?> LoginAsync(LoginDto loginDto);

        // Futuramente, poderia adicionar métodos como:
        // Task<bool> ConfirmarEmailAsync(string userId, string token);
        // Task SolicitarRedefinicaoSenhaAsync(string email);
        // Task<bool> RedefinirSenhaAsync(RedefinirSenhaDto redefinirSenhaDto);
    }
}