namespace ContentManagement.Domain.Aggregates;

public abstract class Persona
{
    public Guid Id { get; protected set; }
    public string Nome { get; protected set; }
    public string Documento { get; protected set; }
    public DateTime DataCriacao { get; protected set; }
    public DateTime? DataAtualizacao { get; protected set; }

    // Relacionamento com Usuario
    public Usuario Usuario { get; protected set; }

    protected Persona() { }

    protected Persona(string nome, string documento)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório", nameof(nome));

        if (string.IsNullOrWhiteSpace(documento))
            throw new ArgumentException("Documento é obrigatório", nameof(documento));

        Nome = nome;
        Documento = documento;
        DataCriacao = DateTime.UtcNow;
    }

    public void Atualizar(string nome, string documento)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório", nameof(nome));

        if (string.IsNullOrWhiteSpace(documento))
            throw new ArgumentException("Documento é obrigatório", nameof(documento));

        Nome = nome;
        Documento = documento;
        DataAtualizacao = DateTime.UtcNow;
    }
}