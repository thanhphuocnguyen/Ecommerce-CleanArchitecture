using System.Security.Claims;
using Ecommerce.Application.Identity.Users.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Results;

namespace Ecommerce.Application.Identity.Interface;

public interface IUserService
{
    Task<PaginationResponse<UserDetailsDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken);

    Task<bool> ExistsWithNameAsync(string name);

    Task<bool> ExistsWithEmailAsync(string email, UserId? exceptId = null);

    Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, UserId? exceptId = null);

    Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken);

    Task<int> GetCountAsync(CancellationToken cancellationToken);

    Task<Result<UserDetailsDto>> GetAsync(UserId userId, CancellationToken cancellationToken);

    Task<Result<List<UserRoleDto>>> GetRolesAsync(UserId userId, CancellationToken cancellationToken);

    Task<Result> AssignRolesAsync(UserId userId, UserRolesRequest request, CancellationToken cancellationToken);

    Task<Result<List<string>>> GetPermissionsAsync(UserId userId, CancellationToken cancellationToken);

    Task<Result<bool>> HasPermissionAsync(UserId userId, string permission, CancellationToken cancellationToken = default);

    Task<Result> InvalidatePermissionCacheAsync(UserId userId, CancellationToken cancellationToken);

    Task<Result> ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken);

    // Task<Result<string>> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal);
    Task<Result<string>> CreateAsync(CreateUserRequest request, string origin);

    Task<Result> UpdateAsync(UpdateUserRequest request, UserId userId);

    Task<Result<string>> ConfirmEmailAsync(UserId userId, string code, CancellationToken cancellationToken);

    Task<Result<string>> ConfirmPhoneAsync(UserId userId, string code, CancellationToken cancellationToken);

    Task<Result<string>> ConfirmChangePhoneNumberAsync(UserId userId, string code);

    Task<Result<string>> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);

    Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest request);

    Task<Result> ChangePasswordAsync(ChangePasswordRequest request, UserId userId);
}