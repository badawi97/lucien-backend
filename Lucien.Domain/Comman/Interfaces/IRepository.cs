using Lucien.Domain.Shared.DI;
using Lucien.Domain.Shared.Entities;

namespace Lucien.Domain.Comman.Interfaces
{
    public interface IRepository<TEntity> : IReadRepository<TEntity>, IWriteRepository<TEntity>, ITransient where TEntity : BaseEntity
    {

    }
}
