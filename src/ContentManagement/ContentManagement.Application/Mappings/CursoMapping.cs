using ContentManagement.Application.DTOs;
using ContentManagement.Domain.Aggregates;
using ContentManagement.Domain.Entities;
using ContentManagement.Domain.ValueObjects;

namespace ContentManagement.Application.Mappings;

public static class CursoMapping
{
    public static CursoDTO ToDTO(this Curso curso)
    {
        return new CursoDTO
        {
            Id = curso.Id,
            Titulo = curso.Titulo,
            Descricao = curso.Descricao,
            Preco = curso.Preco,
            Duracao = curso.Duracao,
            Nivel = curso.Nivel,
            Status = curso.Status,
            DataCriacao = curso.DataCriacao,
            DataAtualizacao = curso.DataAtualizacao,
            Aulas = curso.Aulas.Select(a => a.ToDTO())
        };
    }

    public static AulaDTO ToDTO(this Aula aula)
    {
        return new AulaDTO
        {
            Id = aula.Id,
            Titulo = aula.Titulo,
            Descricao = aula.Descricao,
            Ordem = aula.Ordem,
            ConteudoProgramatico = aula.ConteudoProgramatico.ToDTO()
        };
    }

    public static ConteudoProgramaticoDTO ToDTO(this ConteudoProgramatico conteudo)
    {
        return new ConteudoProgramaticoDTO
        {
            Titulo = conteudo.Titulo,
            Descricao = conteudo.Descricao,
            Ordem = conteudo.Ordem
        };
    }
}