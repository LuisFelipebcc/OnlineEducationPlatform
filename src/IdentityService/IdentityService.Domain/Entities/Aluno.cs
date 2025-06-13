using System;

namespace IdentityService.Domain.Entities
{
    /// <summary>
    /// Representa a Persona de Aluno no sistema.
    /// </summary>
    public class Aluno : Persona
    {
        public DateTime DataMatricula { get; private set; }

        // Outras propriedades específicas do Aluno podem ser adicionadas aqui.
        // Ex: Endereco, Telefone, etc.

        public Aluno(Guid userId, string nomeCompleto, DateTime dataMatricula)
            : base(userId, nomeCompleto)
        {
            DataMatricula = dataMatricula;
        }

        // Construtor privado para uso do ORM.
        private Aluno() : base() { }
    }
}