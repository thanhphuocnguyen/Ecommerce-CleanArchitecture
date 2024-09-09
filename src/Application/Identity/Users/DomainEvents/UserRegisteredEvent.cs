using System;
using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Application.Identity.Users.DomainEvents;

public record UserRegisteredEvent(Guid UserId, string Email, string UserName, string EmailVerificationUrl) : IDomainEvent
{
    public Guid Id => Guid.NewGuid();

    public DateTime OccurredOn => DateTime.UtcNow;
}