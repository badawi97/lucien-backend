using Lucien.Application.Contracts.Permissions.Dtos;
using Lucien.Domain.Shared.DI;

namespace Lucien.Application.Contracts.Permissions.Interfaces
{
    public interface IPermissionApplicationService : ITransient
    {
        IReadOnlyCollection<string> GetCatalog();
        Task<IReadOnlyCollection<PermissionDto>> GetRolePermissionsAsync(Guid roleId);
        Task<PermissionDto> AssignToRoleAsync(Guid roleId, AssignPermissionToRoleDto input);
        Task RemoveFromRoleAsync(Guid roleId, string permissionName);
    }
}
