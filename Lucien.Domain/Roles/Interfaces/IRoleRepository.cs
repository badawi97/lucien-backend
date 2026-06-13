using Lucien.Domain.Comman.Interfaces;
using Lucien.Domain.Roles.Entites;

namespace Lucien.Domain.Roles.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> RenameAsync(Guid id, string name, string actor, CancellationToken cancellationToken = default);
    }
}
