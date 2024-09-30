using Ecommerce.Domain.Common.Events;
using Ecommerce.Domain.Common.Interfaces;
using Ecommerce.Domain.DomainEvents;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Identity.Roles;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Identity;
using Ecommerce.Domain.Shared.Result;
using Ecommerce.Infrastructure.Identity.Entities;
using Ecommerce.Infrastructure.Identity.Extensions;
using Ecommerce.Infrastructure.Persistence;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Infrastructure.Identity.Roles;

public class RoleService : IRoleService
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IEventPublisher _eventPublisher;
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public RoleService(
        RoleManager<AppRole> roleManager,
        IEventPublisher eventPublisher,
        UserManager<AppUser> userManager,
        ApplicationDbContext dbContext,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _roleManager = roleManager;
        _eventPublisher = eventPublisher;
        _userManager = userManager;
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result<Guid>> CreateOrUpdateAsync(CreateOrUpdateRoleRequest request)
    {
        if (request.Id.IsNullOrEmpty())
        {
            var role = new AppRole(request.Name, request.Description);
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return ValidationResult<Guid>.WithErrors(result.GetErrors());
            }

            await _eventPublisher.PublishAsync(new AppRoleCreated(role.Id, role.Name!));

            return role.Id;
        }
        else
        {
            var role = await _roleManager.FindByIdAsync(request.Id);
            if (role == null)
            {
                return Result.Failure<Guid>(DomainErrors.Identity.Role.RoleNotFound);
            }

            if (ERoles.IsDefaultRole(role.Name!))
            {
                return Result.Failure<Guid>(DomainErrors.Identity.Role.DefaultRoleCannotBeUpdated);
            }

            role.Name = request.Name;
            role.NormalizedName = request.Name.ToUpperInvariant();
            role.Description = request.Description;
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                return ValidationResult<Guid>.WithErrors(result.GetErrors());
            }

            await _eventPublisher.PublishAsync(new AppRoleUpdated(role.Id, role.Name!));

            return role.Id;
        }
    }

    public async Task<Result<string>> DeleteAsync(Guid id)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
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

        return Result<string>.Success(role.Id.ToString());
    }

    public async Task<Result<bool>> ExistsAsync(string roleName, Guid? excludeId) =>
        await _roleManager.FindByNameAsync(roleName) is AppRole existingRole && existingRole.Id == excludeId;

    public async Task<Result<RoleDto>> GetByIdAsync(Guid id) =>
        await _dbContext.Roles.SingleOrDefaultAsync(x => x.Id == id) is AppRole role
            ? Result<RoleDto>.Success(role.Adapt<RoleDto>())
            : Result.Failure<RoleDto>(DomainErrors.Identity.Role.RoleNotFound);

    public async Task<Result<RoleDto>> GetByIdWithPermissionsAsync(Guid roleId, CancellationToken cancellationToken)
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

    public async Task<Result<Guid>> UpdatePermissionsAsync(UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.Id.ToString());

        if (role == null)
        {
            return Result.Failure<Guid>(DomainErrors.Identity.Role.RoleNotFound);
        }

        var currentClaims = await _roleManager.GetClaimsAsync(role);

        foreach (var claim in currentClaims.Where(c => !request.Permissions.Any(p => p == c.Value)))
        {
            var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
            if (!removeResult.Succeeded)
            {
                return ValidationResult<Guid>.WithErrors(removeResult.GetErrors());
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
                    CreatedBy = _userContext.Name ?? "Anonymous",
                });
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishAsync(new AppRoleUpdated(role.Id, role.Name!));

        return role.Id;
    }
}