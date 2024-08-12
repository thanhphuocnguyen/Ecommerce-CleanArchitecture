using Ecommerce.Domain.Shared.Primitives;
using MediatR;

namespace Ecommerce.Application.Common.Events;

public abstract class EventNotificationHandler<TEvent> : INotificationHandler<EventNotification<TEvent>>
    where TEvent : IDomainEvent
{
    public Task Handle(EventNotification<TEvent> notification, CancellationToken cancellationToken) =>
        Handle(notification.DomainEvent, cancellationToken);

    public abstract Task Handle(TEvent domainEvent, CancellationToken cancellationToken);
}