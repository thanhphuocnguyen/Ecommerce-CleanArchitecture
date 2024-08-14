using System.Data;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared.Primitives;
using Ecommerce.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

namespace Ecommerce.Infrastructure.Data;

internal sealed class UnitOfWork(ApplicationDbContext dbContext, TimeProvider dateTime) : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly TimeProvider dateTime = dateTime;

    /// <inheritdoc/>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities();
        ConvertDomainEventsToOutboxMessages();

        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    // Must end the transaction by calling Commit or Rollback
    public async Task<IDbTransaction> BeginTransactionAsync()
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync();

        await _dbContext.Database.RollbackTransactionAsync();

        return transaction.GetDbTransaction();
    }

    private void UpdateAuditableEntities()
    {
        foreach (var entry in _dbContext.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                var utcNow = dateTime.GetUtcNow();
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.Created = utcNow;
                }

                entry.Entity.LastModified = utcNow;
            }
        }
    }

    private void ConvertDomainEventsToOutboxMessages()
    {
        var domainEntities = _dbContext.ChangeTracker.Entries<IAggregateRoot>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

        foreach (var entity in domainEntities)
        {
            var events = entity.DomainEvents.Where(e => e is not null).ToArray();
            entity.ClearDomainEvents();

            foreach (var domainEvent in events)
            {
                var outboxMessage = new OutboxMessage(
                    domainEvent.Id,
                    domainEvent.OccurredOn,
                    domainEvent.GetType().Name,
                    JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }));

                _dbContext.Set<OutboxMessage>().Add(outboxMessage);
            }
        }
    }
}

internal static class UoWExtensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}