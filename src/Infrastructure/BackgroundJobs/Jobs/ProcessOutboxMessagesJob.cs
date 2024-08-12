using Ecommerce.Domain.Shared.Primitives;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;

namespace Ecommerce.Infrastructure.BackgroundJobs.Jobs;

[DisallowConcurrentExecution]
internal class ProcessOutboxMessagesJob(
    ApplicationDbContext dbContext,
    TimeProvider timeProvider,
    IPublisher publisher)
    : IJob
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly IPublisher _publisher = publisher;

    public async Task Execute(IJobExecutionContext context)
    {
        var outboxMessages = await _dbContext
            .Set<OutboxMessage>()
            .Where(e => e.ProcessedDate == null)
            .Take(25)
            .ToListAsync(context.CancellationToken);

        foreach (var outboxMessage in outboxMessages)
        {
            var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content);

            if (domainEvent == null)
            {
                continue;
            }

            try
            {
                await _publisher.Publish(domainEvent, context.CancellationToken);

                outboxMessage.ProcessedDate = _timeProvider.GetUtcNow();

                _dbContext.Set<OutboxMessage>().Update(outboxMessage);
            }
            catch (Exception ex)
            {
                outboxMessage.Error = ex.ToString();

                _dbContext.Set<OutboxMessage>().Update(outboxMessage);
            }
        }

        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}