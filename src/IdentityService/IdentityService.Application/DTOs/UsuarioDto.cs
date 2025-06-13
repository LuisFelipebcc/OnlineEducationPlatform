using System;

namespace IdentityService.Application.DTOs
{
    public class UsuarioDto
    {
        public Guid Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string TipoPersona { get; set; } // "Aluno" ou "Admin"
        public string Token { get; set; } // JWT
    }
}