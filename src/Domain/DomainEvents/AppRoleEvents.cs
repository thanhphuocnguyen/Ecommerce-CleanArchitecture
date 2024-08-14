using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Domain.DomainEvents;

#pragma warning disable SA1649 // File name should match first type name
public class AppRoleEvent : DomainEvent
#pragma warning restore SA1649 // File name should match first type name
{
    public string RoleId { get; init; }

    public string RoleName { get; init; }

    protected AppRoleEvent(string roleId, string roleName)
    {
        RoleId = roleId;
        RoleName = roleName;
    }
}

public class AppRoleCreated : AppRoleEvent
{
    public AppRoleCreated(string roleId, string roleName)
        : base(roleId, roleName)
    {
    }
}

public class AppRoleUpdated : AppRoleEvent
{
    public AppRoleUpdated(string roleId, string roleName)
        : base(roleId, roleName)
    {
    }
}

public class AppRoleDeleted : AppRoleEvent
{
    public AppRoleDeleted(string roleId, string roleName)
        : base(roleId, roleName)
    {
    }
}