using OnlineEducationPlatform.API.Models.Identity;

namespace OnlineEducationPlatform.API.Services.Identity;

public interface IIdentityService
{
    Task<TokenResponse> LoginAsync(LoginRequest request);
    Task<TokenResponse> RefreshTokenAsync(string refreshToken);
    Task<bool> ValidateTokenAsync(string token);
}