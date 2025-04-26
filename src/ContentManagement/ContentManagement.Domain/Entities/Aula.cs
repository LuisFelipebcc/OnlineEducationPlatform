using ContentManagement.Domain.ValueObjects;

namespace ContentManagement.Domain.Entities;

public class Aula
{
    public Guid Id { get; private set; }
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public string Conteudo { get; private set; }
    public int Ordem { get; private set; }
    public Guid CursoId { get; private set; }
    public ConteudoProgramatico ConteudoProgramatico { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataAtualizacao { get; private set; }

    private Aula() { }

    public Aula(string titulo, string descricao, string conteudo, int ordem, Guid cursoId, ConteudoProgramatico conteudoProgramatico)
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
        Descricao = descricao;
        Conteudo = conteudo;
        Ordem = ordem;
        CursoId = cursoId;
        ConteudoProgramatico = conteudoProgramatico;
        DataCriacao = DateTime.UtcNow;
    }

    public void Atualizar(string titulo, string descricao, string conteudo, int ordem, ConteudoProgramatico conteudoProgramatico)
    {
        Titulo = titulo;
        Descricao = descricao;
        Conteudo = conteudo;
        Ordem = ordem;
        ConteudoProgramatico = conteudoProgramatico;
        DataAtualizacao = DateTime.UtcNow;
    }
}