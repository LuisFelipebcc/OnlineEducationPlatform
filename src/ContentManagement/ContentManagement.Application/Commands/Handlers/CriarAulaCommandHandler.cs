using ContentManagement.Domain.Entities;
using ContentManagement.Domain.Repositories;
using MediatR;

namespace ContentManagement.Application.Commands.Handlers
{
    public class CriarAulaCommandHandler : IRequestHandler<CriarAulaCommand, Aula>
    {
        private readonly ICursoRepository _cursoRepository;

        public CriarAulaCommandHandler(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<Aula> Handle(CriarAulaCommand request, CancellationToken cancellationToken)
        {
            var curso = await _cursoRepository.GetByIdAsync(request.CursoId);
            if (curso == null)
                throw new ArgumentException("Curso não encontrado");

            var aula = new Aula(request.Titulo, request.Descricao, request.Conteudo, request.Ordem, request.CursoId, null);
            curso.AdicionarAula(aula);

            await _cursoRepository.UpdateAsync(curso);
            return aula;
        }
    }
}