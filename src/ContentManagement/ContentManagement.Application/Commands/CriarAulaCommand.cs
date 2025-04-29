using ContentManagement.Domain.Entities;
using MediatR;

namespace ContentManagement.Application.Commands
{
    public class CriarAulaCommand : IRequest<Aula>
    {
        public Guid CursoId { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Conteudo { get; set; }
        public int Ordem { get; set; }
    }
}
