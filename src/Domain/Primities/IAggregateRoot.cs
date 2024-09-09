namespace Ecommerce.Domain.Shared.Primitives;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();

    void AddDomainEvent(IDomainEvent domainEvent);
}