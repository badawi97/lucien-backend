using Lucien.Application.Contracts.Common.Dto;
using Lucien.Application.Contracts.Users.Dtos;
using Lucien.Application.Contracts.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lucien.HttpApi.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserApplicationService _applicationService;

        public UsersController(IUserApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResultDto<PagedResultDto<UserDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultDto<PagedResultDto<UserDto>>>> GetListAsync([FromQuery] PagedRequestUserDto input)
        {
            var result = await _applicationService.GetListAsync(input);
            return Ok(ResultDto<PagedResultDto<UserDto>>.Success(result, "Users fetched"));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ResultDto<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<UserDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultDto<UserDto>>> GetByIdAsync(Guid id)
        {
            var user = await _applicationService.GetAsync(id);
            if (user == null)
                return NotFound(ResultDto<UserDto>.Failure("User not found", 404));

            return Ok(ResultDto<UserDto>.Success(user, "User retrieved"));
        }

        [HttpGet("by-email")]
        [ProducesResponseType(typeof(ResultDto<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<UserDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultDto<UserDto>>> GetByEmailAsync([FromQuery] string email)
        {
            UserDto? user = await _applicationService.GetByEmailAsync(email);
            if (user == null)
                return NotFound(ResultDto<UserDto>.Failure("User not found", 404));

            return Ok(ResultDto<UserDto>.Success(user, "User retrieved"));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResultDto<UserDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResultDto<UserDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResultDto<UserDto>>> CreateAsync([FromBody] CreateUserDto input)
        {
            var createdUser = await _applicationService.CreateAsync(input);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdUser.Id },
                ResultDto<UserDto>.Success(createdUser, "User created", 201));
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ResultDto<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<UserDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultDto<UserDto>>> UpdateAsync(Guid id, [FromBody] UpdateUserDto input)
        {
            var updatedUser = await _applicationService.UpdateAsync(id, input);
            if (updatedUser == null)
                return NotFound(ResultDto<UserDto>.Failure("User not found", 404));

            return Ok(ResultDto<UserDto>.Success(updatedUser, "User updated"));
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ResultDto<string?>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResultDto<string?>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultDto<string?>>> DeleteAsync(Guid id)
        {
            var existing = await _applicationService.GetAsync(id);
            if (existing == null)
                return NotFound(ResultDto<string?>.Failure("User not found", 404));

            await _applicationService.DeleteAsync(id);
            return Ok(ResultDto<string?>.Success(null, "User deleted", 204));
        }
    }

}
