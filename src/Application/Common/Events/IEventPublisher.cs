using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Domain.Common.Events;

public interface IEventPublisher
{
    Task PublishAsync(IDomainEvent domainEvent);
}