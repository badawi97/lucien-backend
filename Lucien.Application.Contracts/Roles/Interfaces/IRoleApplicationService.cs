using Lucien.Application.Contracts.Common.Dto;
using Lucien.Application.Contracts.Roles.Dtos;
using Lucien.Domain.Shared.DI;

namespace Lucien.Application.Contracts.Roles.Interfaces
{
    public interface IRoleApplicationService : ITransient
    {
        Task<RoleDto> GetAsync(Guid id);
        Task<PagedResultDto<RoleDto>> GetListAsync(PagedRequestRoleDto input);
        Task<RoleDto> CreateAsync(CreateRoleDto input);
        Task<RoleDto> UpdateAsync(Guid id, UpdateRoleDto input);
        Task DeleteAsync(Guid id);
    }
}
