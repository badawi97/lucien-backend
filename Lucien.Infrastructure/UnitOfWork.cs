using Lucien.Domain.Shared.Entities;
using Lucien.Domain.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lucien.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LucienDbContext _dbContext;
        private readonly IMediator _mediator;

        public UnitOfWork(LucienDbContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Persist
            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            // Gather and publish domain events AFTER persistence
            var domainEntities = _dbContext.ChangeTracker
                .Entries()
                .Select(e => e.Entity)
                .OfType<AuditableAggregateRoot<object>>() // or a non-generic base
                .Where(e => e.DomainEvents.Any())
                .ToList();

            var events = domainEntities.SelectMany(e => e.DomainEvents).ToList();
            domainEntities.ForEach(e => e.ClearDomainEvents());

            // If you use an Outbox, write events to Outbox before commit and
            // let a background worker publish. If you do in-process events:
            foreach (var domainEvent in events)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            return result;
        }

        public async Task ExecuteInTransactionAsync(
            Func<CancellationToken, Task> action,
            CancellationToken cancellationToken = default)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                await action(cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await tx.CommitAsync(cancellationToken);
            });
        }
    }
}
