using Lucien.Domain.Shared.Interfaces;

namespace Lucien.Domain.Shared.Entities
{
    public abstract class AuditableAggregateRoot<TEntity> : AuditableEntity<TEntity> where TEntity : class
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent)
            => _domainEvents.Add(domainEvent);

        public void ClearDomainEvents()
            => _domainEvents.Clear();
    }
}
