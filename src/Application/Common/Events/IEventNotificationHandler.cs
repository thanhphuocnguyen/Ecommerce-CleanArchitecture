using Ecommerce.Domain.Shared.Primitives;
using MediatR;

namespace Ecommerce.Domain.Common.Events;

public interface IEventNotificationHandler<TEvent> : INotificationHandler<EventNotification<TEvent>>
    where TEvent : IDomainEvent
{
}