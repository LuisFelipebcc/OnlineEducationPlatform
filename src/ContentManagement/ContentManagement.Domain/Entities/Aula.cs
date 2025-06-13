namespace ContentManagement.Domain.Entities
{
    /// <summary>
    /// Representa uma Aula dentro de um Curso.
    /// </summary>
    public class Aula // Poderia herdar de uma classe base Entity<Guid> se existir em Shared.Domain
    {
        public Guid Id { get; private set; }
        public Guid CursoId { get; private set; } // Chave estrangeira para Curso
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public int DuracaoEmMinutos { get; private set; }
        public int Ordem { get; private set; } // Ordem da aula dentro do curso
        public string UrlVideo { get; private set; } // Ou poderia ser um tipo mais complexo para diferentes tipos de conteúdo

        // Construtor para EF Core
        private Aula() { }

        public Aula(Guid cursoId, string titulo, string descricao, int duracaoEmMinutos, int ordem, string urlVideo)
        {
            Id = Guid.NewGuid();
            CursoId = cursoId;
            Titulo = !string.IsNullOrWhiteSpace(titulo) ? titulo : throw new ArgumentNullException(nameof(titulo));
            Descricao = descricao;
            DuracaoEmMinutos = duracaoEmMinutos > 0 ? duracaoEmMinutos : throw new ArgumentOutOfRangeException(nameof(duracaoEmMinutos), "Duração deve ser positiva.");
            Ordem = ordem > 0 ? ordem : throw new ArgumentOutOfRangeException(nameof(ordem), "Ordem deve ser positiva.");
            UrlVideo = urlVideo; // Pode precisar de validação
        }

        public void AtualizarDetalhes(string novoTitulo, string novaDescricao, int novaDuracaoEmMinutos, string novaUrlVideo)
        {
            if (string.IsNullOrWhiteSpace(novoTitulo))
                throw new ArgumentNullException(nameof(novoTitulo));
            if (novaDuracaoEmMinutos <= 0)
                throw new ArgumentOutOfRangeException(nameof(novaDuracaoEmMinutos), "Duração deve ser positiva.");

            Titulo = novoTitulo;
            Descricao = novaDescricao ?? string.Empty;
            DuracaoEmMinutos = novaDuracaoEmMinutos;
            UrlVideo = novaUrlVideo ?? string.Empty;
        }

        // Se a ordem puder ser alterada independentemente (cuidado com a consistência no Curso)
        // public void AtualizarOrdem(int novaOrdem)
        // {
        //     if (novaOrdem <= 0)
        //         throw new ArgumentOutOfRangeException(nameof(novaOrdem), "Ordem deve ser positiva.");
        //     Ordem = novaOrdem;
        // }
    }
}