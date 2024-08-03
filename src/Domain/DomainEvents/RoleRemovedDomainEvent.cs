using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Domain.DomainEvents;

public record RoleRemovedDomainEvent : IDomainEvent
{
    public Guid Id => Guid.NewGuid();

    public DateTime OccurredOn => DateTime.UtcNow;

    public Role Role { get; }

    public RoleRemovedDomainEvent(Role role)
    {
        Role = role;
        Role = role;
    }
}