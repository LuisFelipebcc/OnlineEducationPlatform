using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OnlineEducationPlatform.API.Models;

namespace OnlineEducationPlatform.API.Services;

public interface IAuthService
{
    Task<AuthResponse> Authenticate(AuthRequest request);
    string GenerateToken(User user);
}

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<AuthResponse> Authenticate(AuthRequest request)
    {
        // TODO: Implementar validação real com banco de dados
        // Por enquanto, vamos usar um usuário mock para teste
        if (request.Email == "admin@test.com" && request.Password == "admin123")
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Username = "Admin",
                Role = "Admin"
            };

            var token = GenerateToken(user);

            return new AuthResponse
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(1),
                Role = user.Role
            };
        }

        throw new UnauthorizedAccessException("Credenciais inválidas");
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}