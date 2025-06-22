using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
