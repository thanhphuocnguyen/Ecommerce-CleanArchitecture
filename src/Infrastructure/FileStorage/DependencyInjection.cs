using Ecommerce.Domain.Common.FileStorage;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.FileStorage;

internal static class DependencyInjection
{
    public static IServiceCollection AddFileStorage(this IServiceCollection services)
    {
        services.AddScoped<IFileStorageService, LocalFileStorageService>();

        return services;
    }
}