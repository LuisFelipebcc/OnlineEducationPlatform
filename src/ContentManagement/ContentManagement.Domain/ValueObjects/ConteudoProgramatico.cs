namespace ContentManagement.Domain.ValueObjects
{
    /// <summary>
    /// Representa o conteúdo programático de um curso.
    /// </summary>
    public class ConteudoProgramatico // Idealmente herdaria de uma classe base ValueObject
    {
        public IReadOnlyList<string> Topicos { get; }
        public IReadOnlyList<string> ObjetivosAprendizagem { get; }

        public ConteudoProgramatico(IEnumerable<string> topicos, IEnumerable<string> objetivosAprendizagem)
        {
            Topicos = topicos?.ToList().AsReadOnly() ?? throw new ArgumentNullException(nameof(topicos));
            ObjetivosAprendizagem = objetivosAprendizagem?.ToList().AsReadOnly() ?? throw new ArgumentNullException(nameof(objetivosAprendizagem));

            if (!Topicos.Any())
                throw new ArgumentException("Conteúdo programático deve ter pelo menos um tópico.", nameof(topicos));
        }

        // Para EF Core, um construtor privado sem parâmetros pode ser necessário se for um Owned Type.
        // Ou, se não for Owned Type e for uma tabela separada, precisaria de um ID.
        // Para simplificar, vamos assumir que será um Owned Type ou serializado.

        // Value Objects devem implementar Equals e GetHashCode
        public override bool Equals(object obj)
        {
            return obj is ConteudoProgramatico outro &&
                   Topicos.SequenceEqual(outro.Topicos) &&
                   ObjetivosAprendizagem.SequenceEqual(outro.ObjetivosAprendizagem);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Topicos, ObjetivosAprendizagem);
        }
    }
}