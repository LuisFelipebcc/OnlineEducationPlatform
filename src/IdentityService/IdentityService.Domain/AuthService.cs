using IdentityService.Domain.Entities;
using IdentityService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Domain
{
    public class AuthService : IAuthService
    {
        private readonly IdentityDbContext _context;

        public AuthService(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<TokenDTO> LoginAsync(LoginDTO loginDTO)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (usuario == null || usuario.Senha != loginDTO.Senha)
                throw new UnauthorizedAccessException("Credenciais inválidas.");

            var token = GerarToken(usuario);
            return new TokenDTO
            {
                Token = token,
                ExpiraEm = DateTime.UtcNow.AddHours(1)
            };
        }

        public async Task<UsuarioDTO> RegistrarAsync(RegistroDTO registroDTO)
        {
            var usuarioExistente = await _context.Usuarios
                .AnyAsync(u => u.Email == registroDTO.Email);

            if (usuarioExistente)
                throw new InvalidOperationException("Usuário já registrado.");

            var usuario = new Usuario(registroDTO.Nome, registroDTO.Email, registroDTO.Senha, registroDTO.Role);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Role = usuario.Role
            };
        }

        public Task<TokenDTO> RefreshTokenAsync(string refreshToken)
        {
            // Implementar lógica de refresh token
            throw new NotImplementedException();
        }

        public Task<bool> ValidarTokenAsync(string token)
        {
            // Implementar lógica de validação de token
            throw new NotImplementedException();
        }

        private string GerarToken(Usuario usuario)
        {
            // Implementar geração de token JWT
            return "token-gerado";
        }
    }
}
