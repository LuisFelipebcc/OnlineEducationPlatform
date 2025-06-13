using Shared.Domain.Interfaces;

namespace Shared.Domain.Entities;

/// <summary>
/// Classe base que implementa o padrão de exclusão lógica (soft delete)
/// </summary>
public abstract class SoftDeleteEntity : ISoftDelete
{
    /// <summary>
    /// Indica se a entidade foi excluída logicamente
    /// </summary>
    public bool IsDeleted { get; protected set; }

    /// <summary>
    /// Data em que a entidade foi excluída logicamente
    /// </summary>
    public DateTime? DeletedAt { get; protected set; }

    /// <summary>
    /// Marca a entidade como excluída logicamente
    /// </summary>
    public virtual void Delete()
    {
        if (!IsDeleted)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Restaura a entidade que foi excluída logicamente
    /// </summary>
    public virtual void Restore()
    {
        if (IsDeleted)
        {
            IsDeleted = false;
            DeletedAt = null;
        }
    }
}