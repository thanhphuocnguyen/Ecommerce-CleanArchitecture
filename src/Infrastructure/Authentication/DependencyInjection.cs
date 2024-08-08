using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Infrastructure.Authentication.Jwt;
using Ecommerce.Infrastructure.Authentication.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Authentication;

internal static class DependencyInjection
{
    internal static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddScoped<UserContextMiddleware>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped(sp => (IUserContextInitializer)sp.GetRequiredService<IUserContext>());

        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddJwt();
        return services;
    }

    internal static IApplicationBuilder UseUserContext(this IApplicationBuilder app)
    {
        app.UseMiddleware<UserContextMiddleware>();
        return app;
    }
}