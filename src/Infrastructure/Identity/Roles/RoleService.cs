using Ecommerce.Application.Common.Events;
using Ecommerce.Application.Identity.Roles;
using Ecommerce.Domain.DomainEvents;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Identity;
using Ecommerce.Domain.Shared.Results;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Data.Extensions;
using Ecommerce.Infrastructure.Identity.Entities;
using Ecommerce.Infrastructure.Identity.Extensions;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Identity.Roles;

public class RoleService : IRoleService
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IEventPublisher _eventPublisher;
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(
        RoleManager<AppRole> roleManager,
        IEventPublisher eventPublisher,
        UserManager<AppUser> userManager,
        ApplicationDbContext dbContext,
        IUnitOfWork unitOfWork)
    {
        _roleManager = roleManager;
        _eventPublisher = eventPublisher;
        _userManager = userManager;
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> CreateOrUpdateAsync(CreateOrUpdateRoleRequest request)
    {
        if (string.IsNullOrEmpty(request.Id))
        {
            var role = new AppRole(request.Name, request.Description);
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return ValidationResult<string>.WithErrors(result.GetErrors());
            }

            await _eventPublisher.PublishAsync(new AppRoleCreated(role.Id, role.Name!));

            return Result<string>.Success(role.Id);
        }
        else
        {
            var role = await _roleManager.FindByIdAsync(request.Id);
            if (role == null)
            {
                return Result.Failure<string>(DomainErrors.Identity.Role.RoleNotFound);
            }

            if (ERoles.IsDefaultRole(role.Name!))
            {
                return Result.Failure<string>(DomainErrors.Identity.Role.DefaultRoleCannotBeUpdated);
            }

            role.Name = request.Name;
            role.NormalizedName = request.Name.ToUpperInvariant();
            role.Description = request.Description;
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                return ValidationResult<string>.WithErrors(result.GetErrors());
            }

            await _eventPublisher.PublishAsync(new AppRoleUpdated(role.Id, role.Name!));

            return Result<string>.Success(role.Id);
        }
    }

    public async Task<Result<string>> DeleteAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return Result.Failure<string>(DomainErrors.Identity.Role.RoleNotFound);
        }

        if (ERoles.IsDefaultRole(role.Name!))
        {
            return Result.Failure<string>(DomainErrors.Identity.Role.DefaultRoleCannotBeDeleted);
        }

        if ((await _userManager.GetUsersInRoleAsync(role.Name!)).Any())
        {
            return Result.Failure<string>(DomainErrors.Identity.Role.RoleHasUsers);
        }

        var result = await _roleManager.DeleteAsync(role);

        if (!result.Succeeded)
        {
            return ValidationResult<string>.WithErrors(result.GetErrors());
        }

        await _eventPublisher.PublishAsync(new AppRoleDeleted(role.Id, role.Name!));

        return Result<string>.Success(role.Id);
    }

    public async Task<Result<bool>> ExistsAsync(string roleName, string? excludeId) =>
        await _roleManager.FindByNameAsync(roleName) is AppRole existingRole && existingRole.Id == excludeId;

    public async Task<Result<RoleDto>> GetByIdAsync(string id) =>
        await _dbContext.Roles.SingleOrDefaultAsync(x => x.Id == id) is AppRole role
            ? Result<RoleDto>.Success(role.Adapt<RoleDto>())
            : Result.Failure<RoleDto>(DomainErrors.Identity.Role.RoleNotFound);

    public async Task<Result<RoleDto>> GetByIdWithPermissionsAsync(string roleId, CancellationToken cancellationToken)
    {
        var role = await GetByIdAsync(roleId);

        if (role.IsFailure)
        {
            return Result.Failure<RoleDto>(role.Error);
        }

        role.Value.Permissions = await _dbContext.RoleClaims
            .Where(x => x.RoleId == roleId && x.ClaimType == EClaims.Permission)
            .Select(x => x.ClaimValue!)
            .ToListAsync(cancellationToken);

        return Result.Success(role.Value);
    }

    public async Task<Result<int>> GetCountAsync(CancellationToken cancellationToken) =>
        await _roleManager.Roles.CountAsync(cancellationToken);

    public async Task<Result<List<RoleDto>>> GetListAsync(CancellationToken cancellationToken) =>
        (await _roleManager.Roles.ToListAsync(cancellationToken)).Adapt<List<RoleDto>>();

    public async Task<Result<string>> UpdatePermissionsAsync(UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.Id);

        if (role == null)
        {
            return Result.Failure<string>(DomainErrors.Identity.Role.RoleNotFound);
        }

        var currentClaims = await _roleManager.GetClaimsAsync(role);

        foreach (var claim in currentClaims.Where(c => !request.Permissions.Any(p => p == c.Value)))
        {
            var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
            if (!removeResult.Succeeded)
            {
                return ValidationResult<string>.WithErrors(removeResult.GetErrors());
            }
        }

        foreach (string permission in request.Permissions.Where(c => !currentClaims.Any(p => p.Value == c)))
        {
            if (!string.IsNullOrEmpty(permission))
            {
                _dbContext.RoleClaims.Add(new AppRoleClaim
                {
                    RoleId = role.Id,
                    ClaimType = EClaims.Permission,
                    ClaimValue = permission,
                    Created = DateTime.UtcNow,
                });
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishAsync(new AppRoleUpdated(role.Id, role.Name!));

        return Result<string>.Success(role.Id);
    }
}