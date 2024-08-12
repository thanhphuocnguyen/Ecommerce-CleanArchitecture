using Ecommerce.Domain.Shared.Primitives;
using MediatR;

namespace Ecommerce.Application.Common.Events;

public class EventNotification<TEvent> : INotification
    where TEvent : IDomainEvent
{
    public EventNotification(TEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

    public TEvent DomainEvent { get; }
}