using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Domain.DomainEvents;

public record RoleAddedDomainEvent : IDomainEvent
{
    public Guid Id => Guid.NewGuid();

    public DateTime OccurredOn => DateTime.UtcNow;

    public RoleAddedDomainEvent(Role role)
    {
        Role = role.Value;
    }

    public int Role { get; }
}