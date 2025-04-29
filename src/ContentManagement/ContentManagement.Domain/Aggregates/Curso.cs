using ContentManagement.Domain.ValueObjects;
using ContentManagement.Domain.Enums;
using ContentManagement.Domain.Entities;

namespace ContentManagement.Domain.Aggregates;

public class Curso
{
    public Guid Id { get; private set; }
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public decimal Preco { get; private set; }
    public int Duracao { get; private set; }
    public string Nivel { get; private set; }
    public StatusCurso Status { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataAtualizacao { get; private set; }
    public ConteudoProgramatico ConteudoProgramatico { get; private set; }

    private readonly List<Aula> _aulas = new();
    public IReadOnlyCollection<Aula> Aulas => _aulas.AsReadOnly();

    private Curso() { }

    public Curso(string titulo, string descricao, decimal preco, int duracao, string nivel, ConteudoProgramatico conteudoProgramatico)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("Título é obrigatório", nameof(titulo));

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição é obrigatória", nameof(descricao));

        if (preco <= 0)
            throw new ArgumentException("Preço deve ser maior que zero", nameof(preco));

        if (duracao <= 0)
            throw new ArgumentException("Duração deve ser maior que zero", nameof(duracao));

        if (string.IsNullOrWhiteSpace(nivel))
            throw new ArgumentException("Nível é obrigatório", nameof(nivel));

        if (conteudoProgramatico == null)
            throw new ArgumentNullException(nameof(conteudoProgramatico));

        Id = Guid.NewGuid();
        Titulo = titulo;
        Descricao = descricao;
        Preco = preco;
        Duracao = duracao;
        Nivel = nivel;
        ConteudoProgramatico = conteudoProgramatico;
        Status = StatusCurso.Ativo;
        DataCriacao = DateTime.UtcNow;
    }

    public void Atualizar(string titulo, string descricao, decimal preco, int duracao, string nivel, ConteudoProgramatico conteudoProgramatico)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("Título é obrigatório", nameof(titulo));

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição é obrigatória", nameof(descricao));

        if (preco <= 0)
            throw new ArgumentException("Preço deve ser maior que zero", nameof(preco));

        if (duracao <= 0)
            throw new ArgumentException("Duração deve ser maior que zero", nameof(duracao));

        if (string.IsNullOrWhiteSpace(nivel))
            throw new ArgumentException("Nível é obrigatório", nameof(nivel));

        if (conteudoProgramatico == null)
            throw new ArgumentNullException(nameof(conteudoProgramatico));

        Titulo = titulo;
        Descricao = descricao;
        Preco = preco;
        Duracao = duracao;
        Nivel = nivel;
        ConteudoProgramatico = conteudoProgramatico;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void AdicionarAula(string titulo, string descricao, string conteudo, int ordem, ConteudoProgramatico conteudoProgramatico)
    {
        var aula = new Aula(titulo, descricao, conteudo, ordem, Id, conteudoProgramatico);
        _aulas.Add(aula);
    }

    public void AtualizarAula(Guid aulaId, string titulo, string descricao, string conteudo, int ordem, ConteudoProgramatico conteudoProgramatico)
    {
        var aula = _aulas.FirstOrDefault(a => a.Id == aulaId);
        if (aula == null)
            throw new ArgumentException("Aula não encontrada", nameof(aulaId));

        aula.Atualizar(titulo, descricao, conteudo, ordem, conteudoProgramatico);
    }

    public void RemoverAula(Guid aulaId)
    {
        var aula = _aulas.FirstOrDefault(a => a.Id == aulaId);
        if (aula == null)
            throw new ArgumentException("Aula não encontrada", nameof(aulaId));

        _aulas.Remove(aula);
    }

    public void Desativar()
    {
        Status = StatusCurso.Inativo;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void Ativar()
    {
        Status = StatusCurso.Ativo;
        DataAtualizacao = DateTime.UtcNow;
    }
}
