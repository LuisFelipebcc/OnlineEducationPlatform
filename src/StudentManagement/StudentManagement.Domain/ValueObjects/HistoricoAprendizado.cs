using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagement.Domain.ValueObjects
{
    public class HistoricoAprendizado // Idealmente herdaria de uma classe base ValueObject
    {
        // Key: AulaId, Value: Data de Conclusão (ou bool para simplesmente concluída)
        public IReadOnlyDictionary<Guid, DateTime?> ProgressoAulas { get; }
        public DateTime? DataConclusaoCurso { get; }

        private readonly Dictionary<Guid, DateTime?> _progressoAulasInternal;

        public HistoricoAprendizado()
        {
            _progressoAulasInternal = new Dictionary<Guid, DateTime?>();
            ProgressoAulas = _progressoAulasInternal.AsReadOnly();
        }

        // Construtor para recriar o objeto (ex: do banco de dados)
        public HistoricoAprendizado(Dictionary<Guid, DateTime?> progressoAulas, DateTime? dataConclusaoCurso)
        {
            _progressoAulasInternal = new Dictionary<Guid, DateTime?>(progressoAulas);
            ProgressoAulas = _progressoAulasInternal.AsReadOnly();
            DataConclusaoCurso = dataConclusaoCurso;
        }

        public HistoricoAprendizado MarcarAulaConcluida(Guid aulaId)
        {
            var novoProgresso = new Dictionary<Guid, DateTime?>(_progressoAulasInternal);
            novoProgresso[aulaId] = DateTime.UtcNow;
            return new HistoricoAprendizado(novoProgresso, DataConclusaoCurso);
        }

        public HistoricoAprendizado MarcarCursoConcluido()
        {
            return new HistoricoAprendizado(_progressoAulasInternal, DateTime.UtcNow);
        }

        // Value Objects devem implementar Equals e GetHashCode
        public override bool Equals(object obj) =>
            obj is HistoricoAprendizado outro &&
            ProgressoAulas.Count == outro.ProgressoAulas.Count &&
            !ProgressoAulas.Except(outro.ProgressoAulas).Any() && // Compara dicionários
            DataConclusaoCurso == outro.DataConclusaoCurso;

        public override int GetHashCode() => HashCode.Combine(ProgressoAulas, DataConclusaoCurso);
    }
}