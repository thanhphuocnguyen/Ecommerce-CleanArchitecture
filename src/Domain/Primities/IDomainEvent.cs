namespace Ecommerce.Domain.Shared.Primitives;

public interface IDomainEvent
{
    Guid Id { get; }

    DateTime OccurredOn { get; }
}