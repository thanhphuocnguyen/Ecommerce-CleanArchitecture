using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Domain.Shared.Primitives;

public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot
    where TKey : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected AggregateRoot(TKey id)
    : base(id)
    {
    }

    protected AggregateRoot()
    {
    }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}