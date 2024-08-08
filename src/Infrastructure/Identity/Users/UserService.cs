using System.Security.Claims;
using Ecommerce.Application.Common.FileStorage;
using Ecommerce.Application.Identity.Interface;
using Ecommerce.Application.Identity.Users.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Shared;
using Ecommerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Infrastructure.Identity.Users;

/// <summary>
/// Represents a user service implementation.
/// </summary>
internal partial class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IFileStorageService _fileStorageService;

    public UserService(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IFileStorageService fileStorageService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _fileStorageService = fileStorageService;
    }

    public Task ChangePasswordAsync(ChangePasswordRequest request, UserId userId)
    {
        throw new NotImplementedException();
    }

    public Task<string> ConfirmEmailAsync(UserId userId, string code, string tenant, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> ConfirmPhoneNumberAsync(UserId userId, string code)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsWithNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null)
    {
        throw new NotImplementedException();
    }

    public Task<UserDetailsDto> GetAsync(UserId userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetCountAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetPermissionsAsync(UserId userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasPermissionAsync(UserId userId, string permission, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task InvalidatePermissionCacheAsync(UserId userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> ResetPasswordAsync(ResetPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<PaginationResponse<UserDetailsDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}