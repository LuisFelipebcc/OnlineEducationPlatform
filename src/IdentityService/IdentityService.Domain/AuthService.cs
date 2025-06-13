using IdentityService.Domain.DTOs;
using IdentityService.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Domain
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TokenDTO> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid email or password");
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!result)
            {
                throw new InvalidOperationException("Invalid email or password");
            }

            var token = await GenerateJwtTokenAsync(user, loginDTO.RememberMe);
            return token;
        }

        public async Task<UserDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO.Password != registerDTO.ConfirmPassword)
            {
                throw new InvalidOperationException("Passwords do not match");
            }

            var user = new IdentityUser
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await _userManager.AddToRoleAsync(user, "Student");

            return new UserDTO
            {
                Id = user.Id,
                Name = registerDTO.Name,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                CreatedAt = DateTime.UtcNow,
                Roles = new List<string> { "Student" }
            };
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

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

        public async Task<bool> RevokeTokenAsync(string token)
        {
            // Implementar lógica de revogação de token
            return true;
        }

        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                CreatedAt = DateTime.UtcNow,
                Roles = roles.ToList()
            };
        }

        public async Task<bool> UpdateUserAsync(string userId, UserDTO userDTO)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            user.Email = userDTO.Email;
            user.PhoneNumber = userDTO.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> ResetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // Implementar envio de email com token
            return true;
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        private async Task<TokenDTO> GenerateJwtTokenAsync(IdentityUser user, bool rememberMe)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var expires = rememberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddHours(1);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );

            return new TokenDTO
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                TokenType = "Bearer",
                ExpiresIn = (int)(expires - DateTime.UtcNow).TotalSeconds
            };
        }
    }
}
