using Lucien.Application.Contracts.Common.Dto;

namespace Lucien.Application.Contracts.Roles.Dtos
{
    public class PagedRequestRoleDto : PagedRequestDto
    {
        public string? Name { get; set; }
    }
}
