using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; }
    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public ApplicationUser()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        RefreshToken = string.Empty;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public void UpdateRefreshToken(string refreshToken, DateTime expiryTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = expiryTime;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}