using ContentManagement.Application.DTOs;
using ContentManagement.Application.Mappings;
using ContentManagement.Domain.Aggregates;
using ContentManagement.Domain.Repositories;
using MediatR;

namespace ContentManagement.Application.Commands.Handlers;

public class CriarCursoCommandHandler : IRequestHandler<CriarCursoCommand, CursoDTO>
{
    private readonly ICursoRepository _cursoRepository;

    public CriarCursoCommandHandler(ICursoRepository cursoRepository)
    {
        _cursoRepository = cursoRepository;
    }

    public async Task<CursoDTO> Handle(CriarCursoCommand request, CancellationToken cancellationToken)
    {
        var curso = new Curso(
            request.DTO.Titulo,
            request.DTO.Descricao,
            request.DTO.Preco
        );

        await _cursoRepository.AdicionarAsync(curso);

        return curso.ToDTO();
    }
}