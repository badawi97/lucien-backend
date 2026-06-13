using Lucien.Application.Contracts.Common.Dto;
using Lucien.Application.Contracts.Permissions.Dtos;
using Lucien.Application.Contracts.Permissions.Interfaces;
using Lucien.Application.Contracts.Roles.Dtos;
using Lucien.Application.Contracts.Roles.Interfaces;
using Lucien.Domain.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lucien.HttpApi.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleApplicationService _applicationService;
        private readonly IPermissionApplicationService _permissionApplicationService;

        public RolesController(IRoleApplicationService applicationService, IPermissionApplicationService permissionApplicationService)
        {
            _applicationService = applicationService;
            _permissionApplicationService = permissionApplicationService;
        }

        [HttpGet]
        [Authorize(Policy = PermissionNames.RolesRead)]
        [ProducesResponseType(typeof(ResultDto<PagedResultDto<RoleDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultDto<PagedResultDto<RoleDto>>>> GetListAsync([FromQuery] PagedRequestRoleDto input)
        {
            var result = await _applicationService.GetListAsync(input);
            return Ok(ResultDto<PagedResultDto<RoleDto>>.Success(result, "Roles fetched"));
        }

        [HttpGet("{id:guid}")]
        [Authorize(Policy = PermissionNames.RolesRead)]
        [ProducesResponseType(typeof(ResultDto<RoleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<RoleDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultDto<RoleDto>>> GetByIdAsync(Guid id)
        {
            var role = await _applicationService.GetAsync(id);
            if (role == null)
                return NotFound(ResultDto<RoleDto>.Failure("Role not found", 404));

            return Ok(ResultDto<RoleDto>.Success(role, "Role retrieved"));
        }

        [HttpPost]
        [Authorize(Policy = PermissionNames.RolesCreate)]
        [ProducesResponseType(typeof(ResultDto<RoleDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResultDto<RoleDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResultDto<RoleDto>>> CreateAsync([FromBody] CreateRoleDto input)
        {
            var createdRole = await _applicationService.CreateAsync(input);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdRole.Id },
                ResultDto<RoleDto>.Success(createdRole, "Role created", 201));
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = PermissionNames.RolesUpdate)]
        [ProducesResponseType(typeof(ResultDto<RoleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<RoleDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultDto<RoleDto>>> UpdateAsync(Guid id, [FromBody] UpdateRoleDto input)
        {
            var updatedRole = await _applicationService.UpdateAsync(id, input);
            if (updatedRole == null)
                return NotFound(ResultDto<RoleDto>.Failure("Role not found", 404));

            return Ok(ResultDto<RoleDto>.Success(updatedRole, "Role updated"));
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = PermissionNames.RolesDelete)]
        [ProducesResponseType(typeof(ResultDto<string?>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResultDto<string?>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultDto<string?>>> DeleteAsync(Guid id)
        {
            var existing = await _applicationService.GetAsync(id);
            if (existing == null)
                return NotFound(ResultDto<string?>.Failure("Role not found", 404));

            await _applicationService.DeleteAsync(id);
            return Ok(ResultDto<string?>.Success(null, "Role deleted", 204));
        }

        [HttpGet("{roleId:guid}/permissions")]
        [Authorize(Policy = PermissionNames.PermissionsRead)]
        [ProducesResponseType(typeof(ResultDto<IReadOnlyCollection<PermissionDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultDto<IReadOnlyCollection<PermissionDto>>>> GetPermissionsAsync(Guid roleId)
        {
            var permissions = await _permissionApplicationService.GetRolePermissionsAsync(roleId);
            return Ok(ResultDto<IReadOnlyCollection<PermissionDto>>.Success(permissions, "Role permissions fetched"));
        }

        [HttpPost("{roleId:guid}/permissions")]
        [Authorize(Policy = PermissionNames.PermissionsCreate)]
        [ProducesResponseType(typeof(ResultDto<PermissionDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResultDto<PermissionDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResultDto<PermissionDto>>> AssignPermissionAsync(Guid roleId, [FromBody] AssignPermissionToRoleDto input)
        {
            var permission = await _permissionApplicationService.AssignToRoleAsync(roleId, input);
            return CreatedAtAction(nameof(GetPermissionsAsync), new { roleId },
                ResultDto<PermissionDto>.Success(permission, "Permission assigned", 201));
        }

        [HttpDelete("{roleId:guid}/permissions/{permissionName}")]
        [Authorize(Policy = PermissionNames.PermissionsDelete)]
        [ProducesResponseType(typeof(ResultDto<string?>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultDto<string?>>> RemovePermissionAsync(Guid roleId, string permissionName)
        {
            await _permissionApplicationService.RemoveFromRoleAsync(roleId, permissionName);
            return Ok(ResultDto<string?>.Success(null, "Permission removed"));
        }
    }
}
