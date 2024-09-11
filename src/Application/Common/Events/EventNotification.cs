using Ecommerce.Domain.Shared.Primitives;
using MediatR;

namespace Ecommerce.Domain.Common.Events;

public class EventNotification<TEvent> : INotification
    where TEvent : IDomainEvent
{
    public EventNotification(TEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

    public TEvent DomainEvent { get; }
}