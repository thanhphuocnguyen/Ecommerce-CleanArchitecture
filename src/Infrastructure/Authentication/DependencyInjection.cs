using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Infrastructure.Authentication.Jwt;
using Ecommerce.Infrastructure.Authentication.Permissions;
using Ecommerce.Infrastructure.Identity.DependencyInjection;
using Ecommerce.Infrastructure.Identity.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Authentication;

internal static class DependencyInjection
{
    internal static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services
            .AddUserContext()
            .AddPermissions()
            .AddIdentity()
            .AddJwt();
        return services;
    }

    internal static IApplicationBuilder UseUserContext(this IApplicationBuilder app) => app.UseMiddleware<UserContextMiddleware>();

    internal static IServiceCollection AddUserContext(this IServiceCollection services)
    {
        services
            .AddScoped<UserContextMiddleware>()
            .AddScoped<IUserContext, UserContext>()
            .AddScoped(sp => (IUserContextInitializer)sp.GetRequiredService<IUserContext>());

        return services;
    }

    internal static IServiceCollection AddPermissions(this IServiceCollection services)
    {
        services
            .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
            .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        return services;
    }
}