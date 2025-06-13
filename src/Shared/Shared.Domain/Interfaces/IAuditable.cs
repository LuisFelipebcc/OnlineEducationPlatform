using System;

namespace Shared.Domain.Interfaces
{
    public interface IAuditable
    {
        DateTime CriadoEm { get; set; } // Ou CreatedAt, para consistência
        string? CriadoPor { get; set; } // ID ou nome do usuário
        DateTime? ModificadoEm { get; set; } // Ou UpdatedAt
        string? ModificadoPor { get; set; } // ID ou nome do usuário
    }
}