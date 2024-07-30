using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Authentication;

public sealed class PermissionService(ApplicationDbContext dbContext) : IPermissionService
{
    public async Task<HashSet<string>> GetPermissionsForUser(UserId userId)
    {
        var roles = await dbContext.Set<User>()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.Id == userId)
            .Select(u => u.Roles)
            .ToArrayAsync();

        return roles
            .SelectMany(r => r)
            .SelectMany(r => r.Permissions)
            .Select(p => p.Name)
            .ToHashSet();
    }
}