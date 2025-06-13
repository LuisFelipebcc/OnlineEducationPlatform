using System.ComponentModel.DataAnnotations; // Para atributos de validação

namespace ContentManagement.Application.DTOs
{
    public class CriarCursoDto
    {
        [Required(ErrorMessage = "O título do curso é obrigatório.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 200 caracteres.")]
        public string Titulo { get; set; }
        public string? Descricao { get; set; } // Descrição pode ser opcional
        public decimal Preco { get; set; }
        public ConteudoProgramaticoDto ConteudoProgramatico { get; set; }
        // O InstrutorId virá do usuário autenticado (Admin)
    }
}