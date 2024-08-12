using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Shared.Identity;
using Ecommerce.Domain.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Identity.Users;

/// <summary>
/// Represents a user service.
/// </summary>
internal partial class UserService
{
    public async Task<Result<List<string>>> GetPermissionsAsync(UserId userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.Value.ToString());

        if (user == null)
        {
            return Result<List<string>>.Failure(DomainErrors.Identity.User.UserNotFound);
        }

        var roles = await _userManager.GetRolesAsync(user);
        var permissions = new List<string>();
        foreach (var role in await _roleManager.Roles
            .Where(r => roles.Contains(r.Name!))
            .ToListAsync(cancellationToken))
        {
            permissions.AddRange(await _dbContext.RoleClaims
                .Where(rc => rc.RoleId == role.Id && rc.ClaimType == EClaims.Permission)
                .Select(rc => rc.ClaimValue!)
                .ToListAsync(cancellationToken));
        }

        return Result<List<string>>.Success(permissions.Distinct().ToList());
    }

    public async Task<Result<bool>> HasPermissionAsync(UserId userId, string permission, CancellationToken cancellationToken = default)
    {
        var permissions = await GetPermissionsAsync(userId, cancellationToken);

        return permissions.IsSuccess && permissions.Value.Contains(permission);
    }

    public Task<Result> InvalidatePermissionCacheAsync(UserId userId, CancellationToken cancellationToken)
    {
        _cacheService.RemoveAsync(_cacheKeys.GetCacheKey(EClaims.Permission, userId), cancellationToken);
        return Task.FromResult(Result.Success());
    }
}