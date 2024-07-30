using Ecommerce.Domain.Entities;

namespace Ecommerce.Infrastructure.Authentication;

public interface IPermissionService
{
    Task<HashSet<string>> GetPermissionsForUser(UserId userId);
}