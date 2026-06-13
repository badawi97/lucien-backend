using AutoMapper;
using Lucien.Application.Contracts.Common.Dto;
using Lucien.Application.Contracts.Roles.Dtos;
using Lucien.Application.Contracts.Roles.Interfaces;
using Lucien.Domain.Roles.Entites;
using Lucien.Domain.Roles.Interfaces;
using Lucien.Domain.Shared.Interfaces;

namespace Lucien.Application.Roles
{
    public class RoleApplicationService : IRoleApplicationService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public RoleApplicationService(IRoleRepository roleRepository, IUserContextService userContextService, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public async Task<PagedResultDto<RoleDto>> GetListAsync(PagedRequestRoleDto input)
        {
            IQueryable<Role> baseQuery = _roleRepository.GetQueryable();

            if (!string.IsNullOrEmpty(input.Name))
            {
                baseQuery = baseQuery.Where(role => role.Name.Contains(input.Name));
            }

            Func<IQueryable<Role>, IQueryable<Role>>? sortingFunc = null;
            if (!string.IsNullOrEmpty(input.Sorting))
            {
                switch (input.Sorting)
                {
                    case "Name":
                        sortingFunc = q => q.OrderBy(role => role.Name);
                        break;
                    case "CreatedAt":
                        sortingFunc = q => q.OrderBy(role => role.CreatedAt);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                sortingFunc = q => q.OrderBy(role => role.Name);
            }

            var pagedResult = await _roleRepository.GetListAsync(baseQuery, input.SkipCount, input.MaxResultCount, sortingFunc);

            var dtoList = _mapper.Map<List<RoleDto>>(pagedResult.Items);
            return new PagedResultDto<RoleDto>(pagedResult.TotalCount, dtoList);
        }

        public async Task<RoleDto> CreateAsync(CreateRoleDto input)
        {
            var role = new Role(input.Name!, _userContextService.GetCurrentUserId());
            var createdRole = await _roleRepository.CreateAsync(role);
            return _mapper.Map<RoleDto>(createdRole);
        }

        public async Task<RoleDto> UpdateAsync(Guid id, UpdateRoleDto input)
        {
            var updatedRole = await _roleRepository.RenameAsync(id, input.Name!, _userContextService.GetCurrentUserId());
            return _mapper.Map<RoleDto>(updatedRole);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _roleRepository.DeleteAsync(id);
        }

        public async Task<RoleDto> GetAsync(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            return _mapper.Map<RoleDto>(role);
        }
    }
}
