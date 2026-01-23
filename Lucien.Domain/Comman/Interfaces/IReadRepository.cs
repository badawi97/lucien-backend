using Lucien.Domain.Comman.Entities;
using System.Linq.Expressions;

namespace Lucien.Domain.Comman.Interfaces
{
    public interface IReadRepository<TEntity>
    {
        Task<TEntity> GetByIdAsync(Guid id,
                                   CancellationToken cancellationToken = default);
        Task<PagedResult<TEntity>> GetListAsync(IQueryable<TEntity> baseQuery,
                                                             int skipCount,
                                                             int maxResultCount,
                                                             Func<IQueryable<TEntity>, IQueryable<TEntity>>? sortingFunc = null,
                                                             bool deletedEntities = false,
                                                             CancellationToken cancellationToken = default);
        Task<int> GetCountAsync(IQueryable<TEntity> baseQuery,
                                             CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id,
                               CancellationToken cancellationToken = default);
        IQueryable<TEntity> GetQueryable(bool includeSoftDeleted = false);
    }
}
