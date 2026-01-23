namespace Lucien.Domain.Shared.Interfaces
{
    public interface IDomainEvent
    {
        Guid EventId { get; }
        DateTime OccurredOn { get; }
    }
}
