using Ecommerce.Domain.Shared.Results;

namespace Ecommerce.Application.Identity.Roles;

public interface IRoleService
{
    Task<Result<List<RoleDto>>> GetListAsync(CancellationToken cancellationToken);

    Task<Result<int>> GetCountAsync(CancellationToken cancellationToken);

    Task<Result<bool>> ExistsAsync(string roleName, string? excludeId);

    Task<Result<RoleDto>> GetByIdAsync(string id);

    Task<Result<RoleDto>> GetByIdWithPermissionsAsync(string roleId, CancellationToken cancellationToken);

    Task<Result<string>> CreateOrUpdateAsync(CreateOrUpdateRoleRequest request);

    Task<Result<string>> UpdatePermissionsAsync(UpdateRolePermissionsRequest request, CancellationToken cancellationToken);

    Task<Result<string>> DeleteAsync(string id);
}