using System;
using MediatR;

namespace IdentityService.Application.Events
{
    public class AlunoDaIdentidadeRegistradoEvent : INotification
    {
        public Guid AlunoId { get; }
        public string NomeCompleto { get; }
        public string Email { get; }

        public AlunoDaIdentidadeRegistradoEvent(Guid alunoId, string nomeCompleto, string email)
        {
            AlunoId = alunoId;
            NomeCompleto = nomeCompleto;
            Email = email;
        }
    }
}