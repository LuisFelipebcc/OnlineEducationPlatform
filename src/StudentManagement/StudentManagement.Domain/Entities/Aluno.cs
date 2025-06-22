using StudentManagement.Domain.Enums;

namespace StudentManagement.Domain.Entities
{
    /// <summary>
    /// Representa um Aluno no contexto de gestão de estudantes, matrículas e progresso.
    /// Esta é uma raiz de agregação.
    /// </summary>
    public class Aluno // Poderia herdar de uma classe base AggregateRoot<Guid> se existir em Shared.Domain
    {
        /// <summary>
        /// ID do Aluno, idealmente o mesmo ID do User no IdentityService.
        /// </summary>
        public Guid Id { get; private set; }
        public string NomeCompleto { get; private set; }
        public string Email { get; private set; }

        private readonly List<Matricula> _matriculas = new();
        public IReadOnlyList<Matricula> Matriculas => _matriculas.AsReadOnly();

        private readonly List<Certificado> _certificados = new();
        public IReadOnlyList<Certificado> Certificados => _certificados.AsReadOnly();

        // Construtor para EF Core
        private Aluno() { }

        public Aluno(Guid id, string nomeCompleto, string email)
        {
            Id = id;
            NomeCompleto = !string.IsNullOrWhiteSpace(nomeCompleto) ? nomeCompleto : throw new ArgumentNullException(nameof(nomeCompleto));
            Email = !string.IsNullOrWhiteSpace(email) ? email : throw new ArgumentNullException(nameof(email));
        }

        public Matricula RealizarMatricula(Guid cursoId, decimal precoPago)
        {
            if (_matriculas.Any(m => m.CursoId == cursoId && m.Status == StatusMatricula.Ativa))
                throw new InvalidOperationException("Aluno já está matriculado e ativo neste curso.");

            var novaMatricula = new Matricula(Id, cursoId, precoPago);
            _matriculas.Add(novaMatricula);
            return novaMatricula;
        }

        public void CancelarMatricula(Guid matriculaId)
        {
            var matricula = _matriculas.FirstOrDefault(m => m.Id == matriculaId);
            if (matricula == null)
                throw new KeyNotFoundException("Matrícula não encontrada.");

            matricula.Cancelar();
        }

        public Certificado EmitirCertificado(Guid cursoId, string nomeCurso)
        {
            // Só permite emitir certificado se houver matrícula concluída no curso
            var matriculaConcluida = _matriculas.Any(m => m.CursoId == cursoId && m.Status == StatusMatricula.Concluida);
            if (!matriculaConcluida)
                throw new InvalidOperationException("Aluno não concluiu o curso para emitir certificado.");

            // Evita duplicidade de certificado para o mesmo curso
            if (_certificados.Any(c => c.CursoId == cursoId))
                throw new InvalidOperationException("Certificado já emitido para este curso.");

            var certificado = new Certificado(Id, cursoId, nomeCurso, DateTime.UtcNow);
            _certificados.Add(certificado);
            return certificado;
        }

        // Exemplo de método para registrar progresso em uma aula
        public void RegistrarProgressoAula(Guid matriculaId, Guid aulaId)
        {
            var matricula = _matriculas.FirstOrDefault(m => m.Id == matriculaId);
            if (matricula == null)
                throw new KeyNotFoundException("Matrícula não encontrada.");

            matricula.RegistrarProgressoAula(aulaId);
        }
    }
}