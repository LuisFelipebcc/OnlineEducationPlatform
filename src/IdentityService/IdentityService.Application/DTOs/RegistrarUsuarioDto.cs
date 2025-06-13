using System.ComponentModel.DataAnnotations;

namespace IdentityService.Application.DTOs
{
    public class RegistrarUsuarioDto
    {
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O nome completo deve ter entre 3 e 200 caracteres.")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        [StringLength(256)]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O tipo de persona é obrigatório (Aluno ou Admin).")]
        public string TipoPersona { get; set; } // "Aluno" ou "Admin"

        // Campos adicionais específicos da persona podem ser adicionados aqui se necessário para o registro
        // Ex: public string? Cargo { get; set; } (para Admin)
    }
}