using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Ecommerce.Infrastructure.BackgroundJobs;

internal static class DependencyInjection
{
    internal static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddQuartz();
        services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = true;
        });

        return services;
    }
}