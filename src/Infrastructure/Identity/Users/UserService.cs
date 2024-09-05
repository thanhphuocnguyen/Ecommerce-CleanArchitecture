using Ardalis.Specification.EntityFrameworkCore;
using Ecommerce.Application.Common.Events;
using Ecommerce.Application.Common.FileStorage;
using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Common.Mailing;
using Ecommerce.Application.Identity.Interface;
using Ecommerce.Application.Identity.Users.Contracts;
using Ecommerce.Domain.DomainEvents.Identity;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Result;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Persistence.Specifications;
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
    private readonly IMailService _mailService;
    private readonly IEmailTemplateService _templateService;
    private readonly IFileStorageService _fileStorageService;
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cacheService;
    private readonly ICacheKeyService _cacheKeys;
    private readonly IEventPublisher _eventPublisher;

    public UserService(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        ApplicationDbContext dbContext,
        ICacheService cacheService,
        ICacheKeyService cacheKeys,
        IEventPublisher eventPublisher,
        IFileStorageService fileStorageService,
        IMailService mailService,
        IEmailTemplateService templateService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _fileStorageService = fileStorageService;
        _dbContext = dbContext;
        _cacheService = cacheService;
        _cacheKeys = cacheKeys;
        _eventPublisher = eventPublisher;
        _mailService = mailService;
        _templateService = templateService;
    }

    public async Task<bool> ExistsWithEmailAsync(string email, Guid? exceptId = null)
    {
        return await _userManager.FindByEmailAsync(email) is AppUser user && user.Id != exceptId;
    }

    public async Task<bool> ExistsWithNameAsync(string name)
    {
        return await _userManager.FindByNameAsync(name) is AppUser;
    }

    public async Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, Guid? exceptId = null)
    {
        return await _userManager.Users.AnyAsync(u => u.PhoneNumber == phoneNumber && u.Id != exceptId);
    }

    public async Task<Result<UserDetailsDto>> GetAsync(Guid userId, CancellationToken cancellationToken)
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

    public async Task<Result<PaginationResponse<UserDetailsDto>>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<AppUser>(filter);

        var users = await _userManager.Users
            .WithSpecification(spec)
            .ProjectToType<UserDetailsDto>()
            .ToListAsync(cancellationToken);

        int count = await _userManager.Users.CountAsync(cancellationToken);

        return new PaginationResponse<UserDetailsDto>(users, count, filter.Page, filter.PageSize);
    }

    public async Task<Result> ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
        {
            return Result.Failure(DomainErrors.Identity.User.UserIdIsNotValid);
        }

        var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return Result.Failure(DomainErrors.Identity.User.UserNotFound);
        }

        bool isAdmin = await _userManager.IsInRoleAsync(user, ERoles.Admin);

        if (isAdmin)
        {
            return Result.Failure(DomainErrors.Identity.User.CannotChangeStatusOfAdmin);
        }

        user.IsActive = request.IsActive;

        await _userManager.UpdateAsync(user);

        await _eventPublisher.PublishAsync(new AppUserUpdated(user.Id, user.UserName!));
        return Result.Success();
    }
}