using Ecommerce.Infrastructure.BackgroundJobs.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Ecommerce.Infrastructure.BackgroundJobs;

public static class JobSchedulersSetup
{
    public static async Task StartOutboxScheduler(WebApplication builder)
    {
        var schedulerFactory = builder.Services.GetRequiredService<ISchedulerFactory>();
        var scheduler = await schedulerFactory.GetScheduler();

        var jobOutbox = JobBuilder.Create<ProcessOutboxMessagesJob>()
            .WithIdentity(nameof(ProcessOutboxMessagesJob))
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"{nameof(ProcessOutboxMessagesJob)}-trigger")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithInterval(TimeSpan.FromSeconds(5))
                .RepeatForever())
            .Build();

        await scheduler.ScheduleJob(jobOutbox, trigger);
    }
}