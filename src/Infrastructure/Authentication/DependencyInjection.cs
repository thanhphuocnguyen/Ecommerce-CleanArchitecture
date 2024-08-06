using Ecommerce.Application.Interfaces;
using Ecommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Authentication;

internal static class DependencyInjection
{
    internal static IServiceCollection AddAuthentication(this IServiceCollection services)
    {
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IJwtProvider, JwtProvider>();

        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        return services;
    }
}