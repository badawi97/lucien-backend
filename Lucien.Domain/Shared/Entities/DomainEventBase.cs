using Lucien.Domain.Shared.Interfaces;

namespace Lucien.Domain.Shared.Entities
{
    public abstract record DomainEventBase : IDomainEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    }
}
