using MediatR;
using ContentManagement.Application.DTOs;

namespace ContentManagement.Application.Commands
{
    public class CriarCursoCommand : IRequest<Guid>
    {
        public string Titulo { get; }
        public string? Descricao { get; }
        public decimal Preco { get; }
        public Guid InstrutorId { get; }
        public ConteudoProgramaticoDto ConteudoProgramatico { get; }

        public CriarCursoCommand(
            string titulo,
            string? descricao,
            decimal preco,
            Guid instrutorId,
            ConteudoProgramaticoDto conteudoProgramatico)
        {
            Titulo = !string.IsNullOrWhiteSpace(titulo) ? titulo : throw new ArgumentNullException(nameof(titulo));
            Descricao = descricao;
            Preco = preco >= 0 ? preco : throw new ArgumentOutOfRangeException(nameof(preco));
            InstrutorId = instrutorId != Guid.Empty ? instrutorId : throw new ArgumentException("InstrutorId inválido", nameof(instrutorId));
            ConteudoProgramatico = conteudoProgramatico ?? throw new ArgumentNullException(nameof(conteudoProgramatico));
        }
    }
}
