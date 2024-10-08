using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Common.DependencyInjection;
using Ecommerce.Infrastructure.Persistence.Initialization;
using Ecommerce.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Persistence.DependencyInjection;

internal static class DependencyInjection
{
    internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, nameof(connectionString));

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlite(connectionString);

            // options.UseNpgsql(connectionString);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<AppDbInitializer>();
        services.AddTransient<AppDbSeeder>();
        services.AddRepositories();
        /* TODO: Add custom seeders
        services.AddServices(typeof(ICustomSeeder), ServiceLifetime.Transient);
        */

        return services;
    }

    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}