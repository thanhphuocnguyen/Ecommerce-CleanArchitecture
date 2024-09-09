using Ecommerce.Infrastructure.Authentication;
using Ecommerce.Infrastructure.BackgroundJobs;
using Ecommerce.Infrastructure.Caching;
using Ecommerce.Infrastructure.FileStorage;
using Ecommerce.Infrastructure.Mailing;
using Ecommerce.Infrastructure.Persistence.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(TimeProvider.System);

        services
            .AddPersistence(configuration)
            .AddAuth()
            .AddCaching()
            .AddFileStorage()
            .AddMailingInfrastructure(configuration)
            .AddMemoryCache()
            .AddBackgroundJobs();
        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseUserContext();
        app.UseAuthorization();
        return app;
    }
}