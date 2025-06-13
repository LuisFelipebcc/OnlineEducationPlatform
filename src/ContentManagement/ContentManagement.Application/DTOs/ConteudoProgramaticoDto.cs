using System.ComponentModel.DataAnnotations;

namespace ContentManagement.Application.DTOs
{
    public class ConteudoProgramaticoDto
    {
        [Required(ErrorMessage = "Pelo menos um tópico é obrigatório.")]
        public List<string> Topicos { get; set; } = new List<string>();
        public List<string> ObjetivosAprendizagem { get; set; } = new List<string>();
    }
}