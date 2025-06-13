using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Application.DTOs
{
    public class MarcarAulaConcluidaDto
    {
        [Required]
        public Guid AulaId { get; set; }
    }
}