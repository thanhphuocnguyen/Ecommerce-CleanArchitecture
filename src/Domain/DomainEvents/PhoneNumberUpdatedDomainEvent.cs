using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Domain.DomainEvents;

public class PhoneNumberUpdatedDomainEvent : IDomainEvent
{
    public Guid Id => Guid.NewGuid();

    public DateTime OccurredOn => DateTime.UtcNow;

    public string PhoneNumber { get; }

    public PhoneNumberUpdatedDomainEvent(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }
}