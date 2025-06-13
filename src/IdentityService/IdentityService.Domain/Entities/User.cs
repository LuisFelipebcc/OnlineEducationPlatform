using System;

namespace IdentityService.Domain.Entities
{
    /// <summary>
    /// Representa um usuário no sistema, contendo as credenciais para autenticação.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Identificador único do usuário. Este ID será compartilhado com a Persona.
        /// </summary>
        public Guid Id { get; private set; }

        public string Email { get; private set; }

        public string PasswordHash { get; private set; }

        // Construtor para criação de um novo usuário.
        public User(string email, string passwordHash)
        {
            Id = Guid.NewGuid(); // O ID é gerado na criação.
            Email = email ?? throw new ArgumentNullException(nameof(email));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        }

        // Construtor privado para uso do ORM.
        private User() { }

        // Métodos adicionais podem ser incluídos aqui, como para alterar senha, etc.
    }
}