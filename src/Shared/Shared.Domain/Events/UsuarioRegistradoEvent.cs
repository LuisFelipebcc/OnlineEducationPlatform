using MediatR;

namespace Shared.Domain.Events
{
    public class UsuarioRegistradoEvent : INotification
    {
        public Guid UsuarioId { get; }
        public string NomeCompleto { get; }
        public string Email { get; }

        public UsuarioRegistradoEvent(Guid usuarioId, string nomeCompleto, string email)
        {
            UsuarioId = usuarioId;
            NomeCompleto = nomeCompleto;
            Email = email;
        }
    }
}