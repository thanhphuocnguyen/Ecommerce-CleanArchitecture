using Ecommerce.Domain.Shared.Result;

namespace Ecommerce.Domain.Identity.Roles;

public interface IRoleService
{
    Task<Result<List<RoleDto>>> GetListAsync(CancellationToken cancellationToken);

    Task<Result<int>> GetCountAsync(CancellationToken cancellationToken);

    Task<Result<bool>> ExistsAsync(string roleName, Guid? excludeId);

    Task<Result<RoleDto>> GetByIdAsync(Guid id);

    Task<Result<RoleDto>> GetByIdWithPermissionsAsync(Guid roleId, CancellationToken cancellationToken);

    Task<Result<Guid>> CreateOrUpdateAsync(CreateOrUpdateRoleRequest request);

    Task<Result<Guid>> UpdatePermissionsAsync(UpdateRolePermissionsRequest request, CancellationToken cancellationToken);

    Task<Result<string>> DeleteAsync(Guid id);
}