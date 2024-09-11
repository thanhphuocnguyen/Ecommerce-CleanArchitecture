using Ecommerce.Domain.Identity.Users.Contracts;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Result;

namespace Ecommerce.Domain.Identity.Interface;

public interface IUserService
{
    Task<Result<PaginationResponse<UserDetailsDto>>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken);

    Task<Result> ExistsWithNameAsync(string name);

    Task<Result> ExistsWithEmailAsync(string email, Guid? exceptId = null);

    Task<Result> ExistsWithPhoneNumberAsync(string phoneNumber, Guid? exceptId = null);

    Task<Result<List<UserDetailsDto>>> GetListAsync(CancellationToken cancellationToken);

    Task<Result<int>> GetCountAsync(CancellationToken cancellationToken);

    Task<Result<UserDetailsDto>> GetAsync(Guid userId, CancellationToken cancellationToken);

    Task<Result<List<UserRoleDto>>> GetRolesAsync(Guid userId, CancellationToken cancellationToken);

    Task<Result> AssignRolesAsync(Guid userId, UserRolesRequest request, CancellationToken cancellationToken);

    Task<Result<List<string>>> GetPermissionsAsync(Guid userId, CancellationToken cancellationToken);

    Task<Result<bool>> HasPermissionAsync(Guid userId, string permission, CancellationToken cancellationToken = default);

    Task<Result> InvalidatePermissionCacheAsync(Guid userId, CancellationToken cancellationToken);

    Task<Result> ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken);

    // Task<Result<string>> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal);
    Task<Result<string>> CreateAsync(CreateUserRequest request, string origin);

    Task<Result> UpdateAsync(UpdateUserRequest request, Guid userId);

    Task<Result<string>> ConfirmEmailAsync(Guid userId, string code, CancellationToken cancellationToken);

    Task<Result<string>> ConfirmPhoneAsync(Guid userId, string code, CancellationToken cancellationToken);

    Task<Result<string>> ConfirmChangePhoneNumberAsync(Guid userId, string code);

    Task<Result<string>> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);

    Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest request);

    Task<Result> ChangePasswordAsync(ChangePasswordRequest request, Guid userId);
}