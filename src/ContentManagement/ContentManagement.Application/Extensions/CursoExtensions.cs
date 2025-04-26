using ContentManagement.Domain.Aggregates;
using ContentManagement.Application.DTOs;

namespace ContentManagement.Application.Extensions;

public static class CursoExtensions
{
    public static CursoDTO ToDTO(this Curso curso)
    {
        if (curso == null)
            return null;

        return new CursoDTO
        {
            Id = curso.Id,
            Titulo = curso.Titulo,
            Descricao = curso.Descricao,
            Duracao = curso.Duracao,
            Nivel = curso.Nivel
        };
    }
}