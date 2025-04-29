namespace OnlineEducationPlatform.API.Models.Identity;

public class TokenResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiraEm { get; set; }
    public string Role { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
}