using System;

namespace IdentityService.Domain.Entities
{
    /// <summary>
    /// Representa a Persona de Administrador no sistema.
    /// </summary>
    public class Admin : Persona
    {
        public string Cargo { get; private set; }

        // Outras propriedades específicas do Admin podem ser adicionadas aqui.
        // Ex: NivelAcesso, Departamento, etc.

        public Admin(Guid userId, string nomeCompleto, string cargo)
            : base(userId, nomeCompleto)
        {
            Cargo = cargo ?? throw new ArgumentNullException(nameof(cargo));
        }

        // Construtor privado para uso do ORM.
        private Admin() : base() { }
    }
}