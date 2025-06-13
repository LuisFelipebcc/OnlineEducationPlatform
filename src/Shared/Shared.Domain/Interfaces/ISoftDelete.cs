namespace Shared.Domain.Interfaces;

/// <summary>
/// Interface que define o padrão de exclusão lógica (soft delete)
/// </summary>
public interface ISoftDelete
{
    /// <summary>
    /// Indica se a entidade foi excluída logicamente
    /// </summary>
    bool IsDeleted { get; }

    /// <summary>
    /// Data em que a entidade foi excluída logicamente
    /// </summary>
    DateTime? DeletedAt { get; }

    /// <summary>
    /// Marca a entidade como excluída logicamente
    /// </summary>
    void Delete();

    /// <summary>
    /// Restaura a entidade que foi excluída logicamente
    /// </summary>
    void Restore();
}