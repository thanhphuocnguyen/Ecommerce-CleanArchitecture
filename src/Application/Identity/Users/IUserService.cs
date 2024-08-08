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

    Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null);

    Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null);

    Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken);

    Task<int> GetCountAsync(CancellationToken cancellationToken);

    Task<UserDetailsDto> GetAsync(UserId userId, CancellationToken cancellationToken);

    Task<Result<List<UserRoleDto>>> GetRolesAsync(UserId userId, CancellationToken cancellationToken);

    Task<Result> AssignRolesAsync(UserId userId, UserRolesRequest request, CancellationToken cancellationToken);

    Task<List<string>> GetPermissionsAsync(UserId userId, CancellationToken cancellationToken);

    Task<bool> HasPermissionAsync(UserId userId, string permission, CancellationToken cancellationToken = default);

    Task InvalidatePermissionCacheAsync(UserId userId, CancellationToken cancellationToken);

    Task ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken);

    // Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal);
    Task<Result<string>> CreateAsync(CreateUserRequest request, string origin);

    Task<Result> UpdateAsync(UpdateUserRequest request, UserId userId);

    Task<string> ConfirmEmailAsync(UserId userId, string code, string tenant, CancellationToken cancellationToken);

    Task<string> ConfirmPhoneNumberAsync(UserId userId, string code);

    // Task<string> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
    Task<string> ResetPasswordAsync(ResetPasswordRequest request);

    Task ChangePasswordAsync(ChangePasswordRequest request, UserId userId);
}