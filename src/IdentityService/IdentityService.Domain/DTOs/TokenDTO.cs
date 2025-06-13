namespace IdentityService.Domain.DTOs
{
    public class TokenDTO
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
    }
}