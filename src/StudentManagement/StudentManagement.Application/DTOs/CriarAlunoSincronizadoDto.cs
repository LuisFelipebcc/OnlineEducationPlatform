using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Application.DTOs
{
    public class CriarAlunoSincronizadoDto
    {
        [Required]
        public Guid Id { get; set; } // Mesmo ID do User no IdentityService
        [Required]
        public string NomeCompleto { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}