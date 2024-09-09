using Ecommerce.Infrastructure.BackgroundJobs.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Ecommerce.Infrastructure.BackgroundJobs;

public static class JobSchedulersSetup
{
    public static async Task StartOutboxScheduler(IServiceProvider services)
    {
        var schedulerFactory = services.GetRequiredService<ISchedulerFactory>();
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