using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Application.Common.Events;

public interface IEventPublisher
{
    Task PublishAsync(IDomainEvent domainEvent);
}