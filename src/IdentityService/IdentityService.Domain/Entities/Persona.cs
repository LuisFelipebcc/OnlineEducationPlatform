using System;

namespace IdentityService.Domain.Entities
{
    /// <summary>
    /// Classe base abstrata para as Personas do negócio (Aluno, Admin).
    /// Compartilha o ID com a entidade User.
    /// </summary>
    public abstract class Persona
    {
        /// <summary>
        /// Identificador único da Persona, que é o mesmo do User associado.
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Nome completo da pessoa representada pela Persona.
        /// </summary>
        public string NomeCompleto { get; protected set; }

        protected Persona(Guid userId, string nomeCompleto)
        {
            Id = userId; // Garante que o ID da Persona é o mesmo do User.
            NomeCompleto = nomeCompleto ?? throw new ArgumentNullException(nameof(nomeCompleto));
        }

        // Construtor privado para uso do ORM.
        protected Persona() { }
    }
}