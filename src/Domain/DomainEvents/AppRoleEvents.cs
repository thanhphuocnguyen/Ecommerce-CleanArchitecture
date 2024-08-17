using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Domain.DomainEvents;

#pragma warning disable SA1649 // File name should match first type name
public class AppRoleEvent : DomainEvent
#pragma warning restore SA1649 // File name should match first type name
{
    public Guid RoleId { get; init; }

    public string RoleName { get; init; }

    protected AppRoleEvent(Guid roleId, string roleName)
    {
        RoleId = roleId;
        RoleName = roleName;
    }
}

public class AppRoleCreated : AppRoleEvent
{
    public AppRoleCreated(Guid roleId, string roleName)
        : base(roleId, roleName)
    {
    }
}

public class AppRoleUpdated : AppRoleEvent
{
    public AppRoleUpdated(Guid roleId, string roleName)
        : base(roleId, roleName)
    {
    }
}

public class AppRoleDeleted : AppRoleEvent
{
    public AppRoleDeleted(Guid roleId, string roleName)
        : base(roleId, roleName)
    {
    }
}