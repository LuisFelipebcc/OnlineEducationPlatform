using ContentManagement.Domain.Aggregates;

namespace StudentManagement.Domain.Entities
{
    public class Certificado
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public DateTime DataEmissao { get; private set; }
        public Guid AlunoId { get; private set; }
        public Guid CursoId { get; private set; }

        // Propriedades de navegação
        public virtual Student Aluno { get; private set; }
        public virtual Curso Curso { get; private set; }

        protected Certificado() { }

        public Certificado(string nome, Guid alunoId, Guid cursoId)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do certificado não pode ser vazio.", nameof(nome));

            Id = Guid.NewGuid();
            Nome = nome;
            DataEmissao = DateTime.UtcNow;
            AlunoId = alunoId;
            CursoId = cursoId;
        }
    }
}
