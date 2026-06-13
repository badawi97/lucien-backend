using AutoMapper;
using Lucien.Application.Contracts.Permissions.Dtos;
using Lucien.Application.Contracts.Permissions.Interfaces;
using Lucien.Domain.Roles.Entites;
using Lucien.Domain.Roles.Interfaces;
using Lucien.Domain.Shared.Authorization;
using Lucien.Domain.Shared.Interfaces;

namespace Lucien.Application.Permissions
{
    public class PermissionApplicationService : IPermissionApplicationService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public PermissionApplicationService(IPermissionRepository permissionRepository, IUserContextService userContextService, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public IReadOnlyCollection<string> GetCatalog()
        {
            return PermissionNames.All;
        }

        public async Task<IReadOnlyCollection<PermissionDto>> GetRolePermissionsAsync(Guid roleId)
        {
            var permissions = await _permissionRepository.GetByRoleIdAsync(roleId);
            var permissionDtos = _mapper.Map<List<PermissionDto>>(permissions);

            foreach (var permissionDto in permissionDtos)
            {
                permissionDto.RoleId = roleId;
            }

            return permissionDtos;
        }

        public async Task<PermissionDto> AssignToRoleAsync(Guid roleId, AssignPermissionToRoleDto input)
        {
            EnsureKnownPermission(input.Name);

            var permission = new Permission(new PermissionName(input.Name!), _userContextService.GetCurrentUserId());
            var createdPermission = await _permissionRepository.CreateAsync(roleId, permission, _userContextService.GetCurrentUserId());

            var permissionDto = _mapper.Map<PermissionDto>(createdPermission);
            permissionDto.RoleId = roleId;

            return permissionDto;
        }

        public async Task RemoveFromRoleAsync(Guid roleId, string permissionName)
        {
            EnsureKnownPermission(permissionName);
            await _permissionRepository.RemoveFromRoleAsync(roleId, new PermissionName(permissionName), _userContextService.GetCurrentUserId());
        }

        private static void EnsureKnownPermission(string? permissionName)
        {
            if (string.IsNullOrWhiteSpace(permissionName) || !PermissionNames.All.Contains(permissionName.Trim()))
            {
                throw new ArgumentException("Permission name is not part of the application permission catalog.", nameof(permissionName));
            }
        }

    }
}
