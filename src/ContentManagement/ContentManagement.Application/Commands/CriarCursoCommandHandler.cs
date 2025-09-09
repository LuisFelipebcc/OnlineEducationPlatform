using MediatR;

namespace ContentManagement.Application.Commands
{
    public class CriarCursoCommandHandler : IRequestHandler<CriarCursoCommand, Guid>
    {
        public async Task<Guid> Handle(CriarCursoCommand request, CancellationToken cancellationToken)
        {
            // Lógica para criar o curso e retornar o Guid
            throw new NotImplementedException();
        }
    }
}
