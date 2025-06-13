namespace StudentManagement.Application.DTOs
{
    public class HistoricoAprendizadoDto
    {
        public Dictionary<Guid, DateTime?> ProgressoAulas { get; set; } = new Dictionary<Guid, DateTime?>();
        public DateTime? DataConclusaoCurso { get; set; }
    }
}