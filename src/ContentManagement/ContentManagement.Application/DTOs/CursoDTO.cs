using ContentManagement.Domain.Enums;

namespace ContentManagement.Application.DTOs;

public class CursoDTO
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Duracao { get; set; }
    public string Nivel { get; set; }
    public StatusCurso Status { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    public IEnumerable<AulaDTO> Aulas { get; set; }
}

public class AulaDTO
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public int Ordem { get; set; }
    public ConteudoProgramaticoDTO ConteudoProgramatico { get; set; }
}

public class ConteudoProgramaticoDTO
{
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public int Ordem { get; set; }
}