using Ecommerce.Application;
using Ecommerce.Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(QueryCachingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(TransactionalPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);
        return services;
    }
}