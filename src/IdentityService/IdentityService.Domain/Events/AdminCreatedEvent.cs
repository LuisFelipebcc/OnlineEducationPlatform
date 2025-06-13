using MediatR;

namespace IdentityService.Domain.Events
{
    public class AdminCreatedEvent : INotification
    {
        public Guid AdminId { get; }
        public string Email { get; }
        public string Nome { get; }
        public string NivelAcesso { get; }
        public DateTime DataCriacao { get; }

        public AdminCreatedEvent(
            Guid adminId,
            string email,
            string nome,
            string nivelAcesso)
        {
            AdminId = adminId;
            Email = email;
            Nome = nome;
            NivelAcesso = nivelAcesso;
            DataCriacao = DateTime.UtcNow;
        }
    }
}