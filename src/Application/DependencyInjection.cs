using Ecommerce.Domain.Behaviors;
using Ecommerce.Domain.Common.Events;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Domain.DependencyInjection;

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

        services.AddTransient<IEventPublisher, EventPublisher>();

        services.AddValidatorsFromAssembly(assembly);
        return services;
    }
}