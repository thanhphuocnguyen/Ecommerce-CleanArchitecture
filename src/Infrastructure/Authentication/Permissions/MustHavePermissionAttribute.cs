using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Infrastructure;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource)
    {
        Policy = $"Permissions.{resource}.{action}";
    }
}