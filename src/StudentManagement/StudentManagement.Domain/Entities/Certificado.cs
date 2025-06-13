namespace StudentManagement.Domain.Entities
{
    public class Certificado // Poderia herdar de uma classe base Entity<Guid>
    {
        public Guid Id { get; private set; }
        public Guid AlunoId { get; private set; }
        public Guid CursoId { get; private set; }
        public string NomeCurso { get; private set; }
        public DateTime DataEmissao { get; private set; }
        public string CodigoVerificacao { get; private set; } // Para validar a autenticidade

        // Construtor para EF Core
        private Certificado() { }

        public Certificado(Guid alunoId, Guid cursoId, string nomeCurso, DateTime dataEmissao)
        {
            Id = Guid.NewGuid();
            AlunoId = alunoId;
            CursoId = cursoId;
            NomeCurso = !string.IsNullOrWhiteSpace(nomeCurso) ? nomeCurso : throw new ArgumentNullException(nameof(nomeCurso));
            DataEmissao = dataEmissao;
            CodigoVerificacao = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper(); // Exemplo simples
        }
    }
}