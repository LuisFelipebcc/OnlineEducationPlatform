using System.Threading;
using System.Threading.Tasks;
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
            _cursoRepository = cursoRepository;
        }

        public async Task<Curso> Handle(ObterCursoPorIdQuery request, CancellationToken cancellationToken)
        {
            return await _cursoRepository.GetByIdAsync(request.Id);
        }
    }
}