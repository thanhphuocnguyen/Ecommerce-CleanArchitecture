using Ecommerce.Domain.Shared.Primitives;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Infrastructure.Identity.Entities;

public class AppRoleClaim : IdentityRoleClaim<string>, IAuditableEntity
{
    public DateTimeOffset Created { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}