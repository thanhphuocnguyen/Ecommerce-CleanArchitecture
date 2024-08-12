using Ecommerce.Infrastructure.BackgroundJobs.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Ecommerce.Infrastructure.BackgroundJobs;

internal static class DependencyInjection
{
    internal static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddQuartz(config =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));
            config.AddJob<ProcessOutboxMessagesJob>(jobKey);

            config.AddTrigger(trigger => trigger
                .WithIdentity($"{jobKey.Name}.trigger")
                .StartNow()
                .WithSimpleSchedule(schedule => schedule
                    .WithInterval(TimeSpan.FromSeconds(30))
                    .RepeatForever())
                .ForJob(jobKey));
        });
        return services;
    }
}