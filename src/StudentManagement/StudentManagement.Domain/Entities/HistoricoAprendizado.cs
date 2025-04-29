namespace StudentManagement.Domain.Entities
{
    public class HistoricoAprendizado
    {
        private readonly List<ProgressoCurso> _progressoCursos;
        public IReadOnlyCollection<ProgressoCurso> ProgressoCursos => _progressoCursos.AsReadOnly();

        public HistoricoAprendizado()
        {
            _progressoCursos = new List<ProgressoCurso>();
        }

        public void RegistrarProgresso(Guid cursoId, int percentualConcluido)
        {
            if (percentualConcluido < 0 || percentualConcluido > 100)
                throw new ArgumentException("O percentual de conclusão deve estar entre 0 e 100.", nameof(percentualConcluido));

            var progresso = _progressoCursos.Find(p => p.CursoId == cursoId);
            if (progresso == null)
            {
                progresso = new ProgressoCurso(cursoId, percentualConcluido);
                _progressoCursos.Add(progresso);
            }
            else
            {
                progresso.AtualizarProgresso(percentualConcluido);
            }
        }
    }

    public class ProgressoCurso
    {
        public Guid CursoId { get; private set; }
        public int PercentualConcluido { get; private set; }
        public DateTime DataUltimaAtualizacao { get; private set; }

        public ProgressoCurso(Guid cursoId, int percentualConcluido)
        {
            CursoId = cursoId;
            PercentualConcluido = percentualConcluido;
            DataUltimaAtualizacao = DateTime.UtcNow;
        }

        public void AtualizarProgresso(int percentualConcluido)
        {
            if (percentualConcluido < 0 || percentualConcluido > 100)
                throw new ArgumentException("O percentual de conclusão deve estar entre 0 e 100.", nameof(percentualConcluido));

            PercentualConcluido = percentualConcluido;
            DataUltimaAtualizacao = DateTime.UtcNow;
        }
    }
}
