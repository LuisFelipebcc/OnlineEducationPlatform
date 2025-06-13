using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Application.DTOs
{
    public class RealizarMatriculaDto
    {
        [Required]
        public Guid CursoId { get; set; }
        // O AlunoId virá do usuário autenticado.
        // O PrecoPago pode vir de uma consulta ao Curso ou do PaymentService.
    }
}