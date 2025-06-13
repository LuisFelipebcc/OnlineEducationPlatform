using System.ComponentModel.DataAnnotations;

namespace ContentManagement.Application.DTOs
{
    public class AtualizarCursoDto
    {
        [Required(ErrorMessage = "O título do curso é obrigatório.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 200 caracteres.")]
        public string Titulo { get; set; }
        public string? Descricao { get; set; }
        public decimal? Preco { get; set; } // Preço pode ser opcional na atualização
        public ConteudoProgramaticoDto? ConteudoProgramatico { get; set; }
    }
}