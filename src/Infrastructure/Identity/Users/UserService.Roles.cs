using System;
using Ecommerce.Application.Identity.Users.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Identity.Users;

/// <summary>
/// Represents a partial class for UserService. For roles related operations.
/// </summary>
internal partial class UserService
{
    public async Task<Result> AssignRolesAsync(Guid userId, UserRolesRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return Result.Failure<string>(DomainErrors.General.ArgumentIsNull(nameof(request)));
        }

        var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return Result.Failure<string>(DomainErrors.Identity.User.UserNotFound);
        }

        foreach (var userRole in request.UserRoles)
        {
            if (await _roleManager.FindByNameAsync(userRole.RoleName!) is not null)
            {
                if (userRole.Enabled)
                {
                    if (!await _userManager.IsInRoleAsync(user, userRole.RoleName))
                    {
                        await _userManager.AddToRoleAsync(user, userRole.RoleName);
                    }
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, userRole.RoleName);
                }
            }
        }

        return Result.Success();
    }

    public async Task<Result<List<UserRoleDto>>> GetRolesAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userRoles = new List<UserRoleDto>();

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return Result.Failure<List<UserRoleDto>>(DomainErrors.Identity.User.UserNotFound);
        }

        var roles = await _roleManager.Roles.AsNoTracking().ToListAsync(cancellationToken);
        if (roles is null)
        {
            return Result.Failure<List<UserRoleDto>>(DomainErrors.Identity.Role.RolesNotFound);
        }

        foreach (var role in roles)
        {
            userRoles.Add(new UserRoleDto(role.Id, role.Name!, role.Description, await _userManager.IsInRoleAsync(user, role.Name!)));
        }

        return userRoles;
    }
}