namespace ContentManagement.Domain.ValueObjects;

public class ConteudoProgramatico
{
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public int Ordem { get; private set; }

    private ConteudoProgramatico() { }

    public ConteudoProgramatico(string titulo, string descricao, int ordem)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("O título não pode ser vazio", nameof(titulo));

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("A descrição não pode ser vazia", nameof(descricao));

        if (ordem <= 0)
            throw new ArgumentException("A ordem deve ser maior que zero", nameof(ordem));

        Titulo = titulo;
        Descricao = descricao;
        Ordem = ordem;
    }
}