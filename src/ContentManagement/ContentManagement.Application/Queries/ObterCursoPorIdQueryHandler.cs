using ContentManagement.Domain.Entities;
using ContentManagement.Domain.Repositories;
using MediatR;

namespace ContentManagement.Application.Queries
{
    public class ObterCursoPorIdQueryHandler : IRequestHandler<ObterCursoPorIdQuery, Curso>
    {
        private readonly ICursoRepository _cursoRepository;

        public ObterCursoPorIdQueryHandler(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository ?? throw new ArgumentNullException(nameof(cursoRepository));
        }

        public async Task<Curso> Handle(ObterCursoPorIdQuery request, CancellationToken cancellationToken)
        {
            var curso = await _cursoRepository.GetByIdAsync(request.Id);
            if (curso == null)
                throw new InvalidOperationException($"Curso com ID {request.Id} não encontrado.");

            return curso;
        }
    }
}