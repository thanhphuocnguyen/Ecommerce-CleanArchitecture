using Ecommerce.Application.Common.FileStorage;
using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Common.Mailing;
using Ecommerce.Application.Identity.Interface;
using Ecommerce.Application.Identity.Users.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Results;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Identity.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
    private readonly IJobService _jobService;
    private readonly IMailService _mailService;
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cacheService;
    private readonly ICacheKeyService _cacheKeys;

    public UserService(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IFileStorageService fileStorageService,
        IJobService jobService,
        IMailService mailService,
        ApplicationDbContext dbContext,
        ICacheService cacheService,
        ICacheKeyService cacheKeys)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _fileStorageService = fileStorageService;
        _jobService = jobService;
        _mailService = mailService;
        _dbContext = dbContext;
        _cacheService = cacheService;
        _cacheKeys = cacheKeys;
    }

    public async Task<bool> ExistsWithEmailAsync(string email, UserId? exceptId = null)
    {
        return await _userManager.FindByEmailAsync(email) is AppUser user && user.Id != exceptId;
    }

    public async Task<bool> ExistsWithNameAsync(string name)
    {
        return await _userManager.FindByNameAsync(name) is AppUser;
    }

    public async Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, UserId? exceptId = null)
    {
        return await _userManager.Users.AnyAsync(u => u.PhoneNumber == phoneNumber && u.Id != exceptId);
    }

    public async Task<Result<UserDetailsDto>> GetAsync(UserId userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user == null)
        {
            return Result<UserDetailsDto>.Failure(DomainErrors.Identity.User.UserNotFound);
        }

        return user.Adapt<UserDetailsDto>();
    }

    public Task<int> GetCountAsync(CancellationToken cancellationToken) =>
        _userManager.Users.AsNoTracking().CountAsync(cancellationToken);

    public async Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken)
    {
        return (await _userManager.Users.AsNoTracking().ToListAsync(cancellationToken)).Adapt<List<UserDetailsDto>>();
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