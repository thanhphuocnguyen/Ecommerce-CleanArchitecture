using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Authentication;
using Ecommerce.Infrastructure.BackgroundJobs;
using Ecommerce.Infrastructure.Caching;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, nameof(connectionString));

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlite(connectionString);

            // options.UseNpgsql(connectionString);
        });
        services
            .AddAuth()
            .AddBackgroundJobs();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton(TimeProvider.System);

        services.AddMemoryCache();
        services.AddScoped<ICacheService, CacheService>();

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