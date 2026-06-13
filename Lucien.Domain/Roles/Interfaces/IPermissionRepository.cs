using Lucien.Domain.Roles.Entites;
using Lucien.Domain.Shared.DI;

namespace Lucien.Domain.Roles.Interfaces
{
    public interface IPermissionRepository : ITransient
    {
        Task<IReadOnlyCollection<Permission>> GetByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default);
        Task<Permission> CreateAsync(Guid roleId, Permission permission, string actor, CancellationToken cancellationToken = default);
        Task RemoveFromRoleAsync(Guid roleId, PermissionName name, string actor, CancellationToken cancellationToken = default);
    }
}
