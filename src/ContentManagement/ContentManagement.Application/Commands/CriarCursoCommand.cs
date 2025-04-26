using ContentManagement.Application.DTOs;
using MediatR;

namespace ContentManagement.Application.Commands;

public class CriarCursoCommand : IRequest<CursoDTO>
{
    public CriarCursoDTO DTO { get; set; }
}