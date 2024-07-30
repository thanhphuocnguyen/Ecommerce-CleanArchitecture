using Ecommerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute()
    {
    }

    public HasPermissionAttribute(Permission permission)
    : base(policy: permission.ToString())
    {
    }

    public HasPermissionAttribute(string policy)
    : base(policy)
    {
    }
}