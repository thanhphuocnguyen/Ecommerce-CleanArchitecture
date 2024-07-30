using MediatR;

namespace Ecommerce.Domain.Shared.Primitives;

public interface IDomainEvent : INotification
{
    Guid Id { get; }

    DateTime OccurredOn { get; }
}