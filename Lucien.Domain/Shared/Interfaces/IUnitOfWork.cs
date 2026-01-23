using Lucien.Domain.Shared.DI;

namespace Lucien.Domain.Shared.Interfaces
{
    public interface IUnitOfWork : IScoped
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task ExecuteInTransactionAsync(Func<CancellationToken, Task> action,
                                   CancellationToken cancellationToken = default);
    }
}
