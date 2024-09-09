using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Identity;
using Ecommerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Infrastructure.Persistence.Initialization;

public class AppDbSeeder
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ICustomSeeder _customSeeder;
    private readonly ILogger<AppDbSeeder> _logger;

    public AppDbSeeder(
        RoleManager<AppRole> roleManager,
        UserManager<AppUser> userManager,
        ICustomSeeder customSeeder,
        ILogger<AppDbSeeder> logger)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _customSeeder = customSeeder;
        _logger = logger;
    }

    public async Task SeedAsync(ApplicationDbContext dbContext)
    {
        await SeedRolesAsync(dbContext);
        await SeedAdminAsync();
        await _customSeeder.InitializeAsync();
    }

    private async Task SeedAdminAsync()
    {
        if (await _userManager.FindByNameAsync("admin") is not AppUser user)
        {
            _logger.LogInformation("Seeding admin user...");
            user = new AppUser
            {
                FirstName = "This is",
                LastName = "Root User",
                UserName = "admin",
                Email = "admin@ntp.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = "admin@ntp.com".ToUpperInvariant(),
                NormalizedUserName = "admin".ToUpperInvariant(),
                IsActive = true
            };
            _logger.LogInformation("Creating admin user...");
            var password = new PasswordHasher<AppUser>();
            user.PasswordHash = password.HashPassword(user, "admin");
            await _userManager.CreateAsync(user);
        }

        if (!await _userManager.IsInRoleAsync(user, ERoles.Admin))
        {
            _logger.LogInformation("Assigning admin role to admin user...");
            await _userManager.AddToRoleAsync(user, ERoles.Admin);
        }
    }

    private async Task AssignPermissionToRoleAsync(ApplicationDbContext dbContext, IReadOnlyList<EPermission> perms, AppRole role)
    {
        var currentClaims = await _roleManager.GetClaimsAsync(role);
        foreach (EPermission permission in perms)
        {
            if (!currentClaims.Any(c => c.Type == EClaims.Permission && c.Value == permission.Name))
            {
                _logger.LogInformation("Seeding permission {PermissionName} to role {RoleName}...", permission.Name, role.Name);
                dbContext.RoleClaims.Add(new AppRoleClaim
                {
                    RoleId = role.Id,
                    ClaimType = EClaims.Permission,
                    ClaimValue = permission.Name,
                    CreatedBy = "ApplicationDbSeeder"
                });
            }
        }
    }

    private async Task SeedRolesAsync(ApplicationDbContext dbContext)
    {
        foreach (string roleName in ERoles.DefaultRoles)
        {
            if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName) is not AppRole role)
            {
                _logger.LogInformation("Seeding role {RoleName}...", roleName);
                role = new AppRole(roleName);
                await _roleManager.CreateAsync(role);
            }

            if (roleName == ERoles.User)
            {
                await AssignPermissionToRoleAsync(dbContext, EPermissions.Basic, role);
            }
            else if (roleName == ERoles.Admin)
            {
                await AssignPermissionToRoleAsync(dbContext, EPermissions.All, role);
            }
            else if (roleName == ERoles.Vendor)
            {
                await AssignPermissionToRoleAsync(dbContext, EPermissions.Vendor, role);
            }
        }
    }
}