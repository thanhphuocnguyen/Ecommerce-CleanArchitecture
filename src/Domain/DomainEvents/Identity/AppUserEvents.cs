using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Domain.DomainEvents.Identity;

#pragma warning disable SA1649 // File name should match first type name
public class AppUserEvent : DomainEvent
#pragma warning restore SA1649 // File name should match first type name
{
    public Guid Guid { get; init; }

    public string UserName { get; init; }

    protected AppUserEvent(Guid userId, string userName)
    {
        Guid = userId;
        UserName = userName;
    }
}

public class AppUserCreated : AppUserEvent
{
    public AppUserCreated(Guid userId, string userName)
        : base(userId, userName)
    {
    }
}

public class AppUserUpdated : AppUserEvent
{
    public AppUserUpdated(Guid userId, string userName)
        : base(userId, userName)
    {
    }
}