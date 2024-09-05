using Ecommerce.Domain.Exceptions;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Authentication.Permissions;

internal class PermissionAuthorizationHandler(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    ApplicationDbContext dbContext) : AuthorizationHandler<PermissionRequirement>
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly RoleManager<AppRole> _roleManager = roleManager;
    private readonly ApplicationDbContext _dbContext = dbContext;

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var userId = context.User.GetUserId();

        if (userId is null)
        {
            new UnauthorizedException("Authentication Failed.");
        }

        var user = await _userManager.FindByIdAsync(userId!.Value.ToString());
        if (user is null)
        {
            new UnauthorizedException("Authentication Failed.");
        }

        var userRoles = await _userManager.GetRolesAsync(user!);
        var permissions = new List<string>();
        foreach (var role in await _roleManager.Roles
            .Where(r => userRoles.Contains(r.Name!))
            .ToListAsync(default))
        {
            permissions.AddRange(await _dbContext.RoleClaims
                .Where(rc => rc.RoleId == role.Id && rc.ClaimType == "Permission")
                .Select(rc => rc.ClaimValue!)
                .ToListAsync(default));
        }

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}