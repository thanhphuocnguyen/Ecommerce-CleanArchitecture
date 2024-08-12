using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Domain.DomainEvents;

public class AppRoleEvents : DomainEvent
{
    public string RoleId { get; init; }

    public string RoleName { get; init; }

    protected AppRoleEvents(string roleId, string roleName)
    {
        RoleId = roleId;
        RoleName = roleName;
    }
}

public class AppRoleCreated : AppRoleEvents
{
    public AppRoleCreated(string roleId, string roleName)
        : base(roleId, roleName)
    {
    }
}

public class AppRoleUpdated : AppRoleEvents
{
    public AppRoleUpdated(string roleId, string roleName)
        : base(roleId, roleName)
    {
    }
}

public class AppRoleDeleted : AppRoleEvents
{
    public AppRoleDeleted(string roleId, string roleName)
        : base(roleId, roleName)
    {
    }
}