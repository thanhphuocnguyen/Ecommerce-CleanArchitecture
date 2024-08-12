using Ecommerce.Domain.Shared.Primitives;
using MediatR;

namespace Ecommerce.Application.Common.Events;

public interface IEventNotificationHandler<TEvent> : INotificationHandler<EventNotification<TEvent>>
    where TEvent : IDomainEvent
{
}