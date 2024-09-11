using Ecommerce.Domain.Shared.Primitives;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Domain.Common.Events;

public class EventPublisher(ILogger<EventPublisher> logger, IPublisher publisher) : IEventPublisher
{
    private readonly ILogger<EventPublisher> _logger = logger;
    private readonly IPublisher _publisher = publisher;

    public Task PublishAsync(IDomainEvent domainEvent)
    {
        _logger.LogInformation("Publishing Event : {event}", domainEvent.GetType().Name);
        return _publisher.Publish(CreateEventNotification(domainEvent));
    }

    private static INotification CreateEventNotification(IDomainEvent @event) =>
        (INotification)Activator.CreateInstance(
            typeof(EventNotification<>).MakeGenericType(@event.GetType()), @event)!;
}