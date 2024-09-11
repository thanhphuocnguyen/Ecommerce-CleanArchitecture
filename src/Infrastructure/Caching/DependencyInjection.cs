using Ecommerce.Domain.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Caching;

public static class DependencyInjection
{
    public static IServiceCollection AddCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<ICacheKeyService, CacheKeyService>();

        return services;
    }
}