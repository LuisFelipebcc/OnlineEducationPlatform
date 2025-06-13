using ContentManagement.Domain.ValueObjects;

namespace ContentManagement.Domain.Entities
{
    /// <summary>
    /// Representa um Curso, que é a raiz de agregação para Aulas.
    /// </summary>
    public class Curso // Poderia herdar de uma classe base AggregateRoot<Guid> se existir em Shared.Domain
    {
        public Guid Id { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public Guid InstrutorId { get; private set; } // ID do Admin/Instrutor que criou o curso
        public DateTime DataPublicacao { get; private set; }
        public decimal Preco { get; private set; }
        public ConteudoProgramatico ConteudoProgramatico { get; private set; }

        private readonly List<Aula> _aulas = new List<Aula>();
        public IReadOnlyList<Aula> Aulas => _aulas.AsReadOnly();

        // Construtor para EF Core
        private Curso() { }

        public Curso(string titulo, string descricao, Guid instrutorId, decimal preco, ConteudoProgramatico conteudoProgramatico)
        {
            Id = Guid.NewGuid();
            Titulo = !string.IsNullOrWhiteSpace(titulo) ? titulo : throw new ArgumentNullException(nameof(titulo));
            Descricao = descricao; // Pode ser nulo ou vazio dependendo das regras de negócio
            InstrutorId = instrutorId;
            Preco = preco >= 0 ? preco : throw new ArgumentOutOfRangeException(nameof(preco), "Preço não pode ser negativo.");
            ConteudoProgramatico = conteudoProgramatico ?? throw new ArgumentNullException(nameof(conteudoProgramatico));
            DataPublicacao = DateTime.UtcNow; // Ou pode ser definido explicitamente
        }

        public void AdicionarAula(string tituloAula, string descricaoAula, int duracaoEmMinutos, string urlVideo)
        {
            if (string.IsNullOrWhiteSpace(tituloAula))
                throw new ArgumentNullException(nameof(tituloAula));

            var ordem = _aulas.Any() ? _aulas.Max(a => a.Ordem) + 1 : 1;
            var novaAula = new Aula(Id, tituloAula, descricaoAula, duracaoEmMinutos, ordem, urlVideo);
            _aulas.Add(novaAula);
        }

        public void RemoverAula(Guid aulaId)
        {
            var aulaParaRemover = _aulas.FirstOrDefault(a => a.Id == aulaId);
            if (aulaParaRemover != null)
            {
                _aulas.Remove(aulaParaRemover);
            }
            // Considerar o que fazer se a aula não for encontrada (lançar exceção, retornar bool, etc.)
        }

        public void AtualizarConteudoProgramatico(ConteudoProgramatico novoConteudo)
        {
            ConteudoProgramatico = novoConteudo ?? throw new ArgumentNullException(nameof(novoConteudo));
        }

        public void AtualizarTitulo(string novoTitulo)
        {
            if (string.IsNullOrWhiteSpace(novoTitulo))
                throw new ArgumentNullException(nameof(novoTitulo));
            Titulo = novoTitulo;
        }

        public void AtualizarDescricao(string novaDescricao)
        {
            // Permitir descrição nula ou vazia se a regra de negócio permitir
            Descricao = novaDescricao ?? string.Empty;
        }

        public void AtualizarPreco(decimal novoPreco)
        {
            if (novoPreco < 0)
                throw new ArgumentOutOfRangeException(nameof(novoPreco), "Preço não pode ser negativo.");
            Preco = novoPreco;
        }
        // Outros métodos de negócio para o Curso (ex: PublicarCurso, AlterarPreco, etc.)
    }
}