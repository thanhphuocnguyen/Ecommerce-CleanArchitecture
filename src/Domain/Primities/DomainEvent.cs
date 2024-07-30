namespace Ecommerce.Domain.Shared.Primitives;

public class DomainEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();

    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}