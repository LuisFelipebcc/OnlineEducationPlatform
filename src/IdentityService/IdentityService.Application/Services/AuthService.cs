using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using IdentityService.Application.DTOs;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using IdentityService.Infrastructure.Data;

namespace IdentityService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IdentityDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(IdentityDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<TokenDTO> LoginAsync(LoginDTO loginDTO)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (usuario == null || !VerificarSenha(loginDTO.Senha, usuario.SenhaHash))
                throw new UnauthorizedAccessException("Email ou senha inválidos");

            if (!usuario.Ativo)
                throw new UnauthorizedAccessException("Usuário inativo");

            var token = GerarToken(usuario);
            var refreshToken = GerarRefreshToken();

            usuario.AtualizarRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
            usuario.RegistrarAcesso();

            await _context.SaveChangesAsync();

            return new TokenDTO
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiraEm = DateTime.UtcNow.AddHours(1)
            };
        }

        public async Task<UsuarioDTO> RegistrarAsync(RegistroDTO registroDTO)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == registroDTO.Email))
                throw new InvalidOperationException("Email já cadastrado");

            var senhaHash = HashSenha(registroDTO.Senha);
            var usuario = new Usuario(
                registroDTO.Nome,
                registroDTO.Email,
                senhaHash,
                "User" // Role padrão para novos usuários
            );

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Role = usuario.Role
            };
        }

        public async Task<TokenDTO> RefreshTokenAsync(string refreshToken)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (usuario == null || !usuario.Ativo ||
                !usuario.RefreshTokenExpiraEm.HasValue ||
                usuario.RefreshTokenExpiraEm.Value < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token inválido ou expirado");

            var novoToken = GerarToken(usuario);
            var novoRefreshToken = GerarRefreshToken();

            usuario.AtualizarRefreshToken(novoRefreshToken, DateTime.UtcNow.AddDays(7));
            await _context.SaveChangesAsync();

            return new TokenDTO
            {
                Token = novoToken,
                RefreshToken = novoRefreshToken,
                ExpiraEm = DateTime.UtcNow.AddHours(1)
            };
        }

        public async Task<bool> ValidarTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GerarToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GerarRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        private string HashSenha(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        private bool VerificarSenha(string senha, string senhaHash)
        {
            return BCrypt.Net.BCrypt.Verify(senha, senhaHash);
        }
    }
}