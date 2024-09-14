using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Common.DependencyInjection;

internal static class GenericDependencyInjection
{
    internal static IServiceCollection AddServices(this IServiceCollection services, Type serviceType, ServiceLifetime lifetime)
    {
        var interfaces = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => serviceType.IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)
            .Select(t => new
            {
                Service = t.GetInterfaces().FirstOrDefault(),
                Implementation = t
            })
            .Where(t => t.Service is not null && serviceType.IsAssignableFrom(t.Service));

        foreach (var type in interfaces)
        {
            services.AddService(type.Service!, type.Implementation, lifetime);
        }

        return services;
    }

    internal static IServiceCollection AddService(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime) => lifetime switch
    {
        ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
        ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
        _ => services.AddScoped(serviceType, implementationType)
    };
}