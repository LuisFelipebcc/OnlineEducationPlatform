namespace IdentityService.Domain.DTOs
{
    public class ConfirmEmailDTO
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}