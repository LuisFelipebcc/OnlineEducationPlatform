namespace Shared.Domain.Entities;

/// <summary>
/// Classe base para todas as entidades que são raízes de agregação
/// </summary>
public abstract class AggregateRoot : SoftDeleteEntity
{
    private readonly List<DomainEvent> _events = new();
    public IReadOnlyCollection<DomainEvent> Events => _events.AsReadOnly();

    protected void AddDomainEvent(DomainEvent @event)
    {
        _events.Add(@event);
    }

    public void ClearEvents()
    {
        _events.Clear();
    }
}