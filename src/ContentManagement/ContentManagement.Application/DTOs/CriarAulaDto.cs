using System.ComponentModel.DataAnnotations;

namespace ContentManagement.Application.DTOs
{
    public class CriarAulaDto
    {
        [Required(ErrorMessage = "O título da aula é obrigatório.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 200 caracteres.")]
        public string Titulo { get; set; }
        public string? Descricao { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "A duração deve ser de pelo menos 1 minuto.")]
        public int DuracaoEmMinutos { get; set; }
        public string? UrlVideo { get; set; }
    }
}