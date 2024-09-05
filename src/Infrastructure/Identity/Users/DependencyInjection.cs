using Ecommerce.Application.Identity.Interface;
using Ecommerce.Application.Identity.Roles;
using Ecommerce.Application.Identity.Tokens;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Identity.Entities;
using Ecommerce.Infrastructure.Identity.Roles;
using Ecommerce.Infrastructure.Identity.Tokens;
using Ecommerce.Infrastructure.Identity.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Identity.DependencyInjection;

internal static class DependencyInjection
{
    internal static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>(opt =>
        {
            opt.Password.RequireDigit = false;
            opt.Password.RequiredLength = 6;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequiredUniqueChars = 0;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IUserService, UserService>();

        return services;
    }
}