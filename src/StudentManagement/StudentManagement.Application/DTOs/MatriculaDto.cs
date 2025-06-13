using System;

namespace StudentManagement.Application.DTOs
{
    public class MatriculaDto
    {
        public Guid Id { get; set; }
        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }
        public string NomeCurso { get; set; } // Adicionado para conveniência
        public DateTime DataMatricula { get; set; }
        public string Status { get; set; } // Modificado de bool Ativa para string (representando o enum)
        public decimal PrecoPago { get; set; }
        public HistoricoAprendizadoDto? HistoricoAprendizado { get; set; }
    }
}