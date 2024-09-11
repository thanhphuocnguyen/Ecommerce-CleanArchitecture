using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Domain.Identity.DomainEvents;

public record PasswordResetRequestedEvent : IDomainEvent
{
    public Guid Id => Guid.NewGuid();

    public DateTime OccurredOn => DateTime.UtcNow;

    public Guid UserId { get; init; }

    public string Email { get; init; } = null!;

    public string ResetPasswordUrl { get; init; } = null!;

    public PasswordResetRequestedEvent(
        Guid userId,
        string email,
        string resetPasswordUrl)
    {
        UserId = userId;
        Email = email;
        ResetPasswordUrl = resetPasswordUrl;
    }
}