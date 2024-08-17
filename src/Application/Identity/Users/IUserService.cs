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

    Task<bool> ExistsWithEmailAsync(string email, Guid? exceptId = null);

    Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, Guid? exceptId = null);

    Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken);

    Task<int> GetCountAsync(CancellationToken cancellationToken);

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