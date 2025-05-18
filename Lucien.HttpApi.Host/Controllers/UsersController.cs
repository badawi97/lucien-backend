using Lucien.Application.Contracts.Users.Dtos;
using Lucien.Application.Contracts.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lucien.HttpApi.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserApplicationService _applicationService;

        public UsersController(IUserApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<UserDto>> GetLsitAsync()
        {
            return await _applicationService.GetListAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<UserDto> GetAsync(Guid id)
        {
            return await _applicationService.GetAsync(id);
        }

        [HttpPost]
        [Authorize]
        public async Task<UserDto> CreateAsync([FromBody] CreateUserDto input)
        {
            return await _applicationService.CreateAsync(input);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<UserDto> UpdateAsync(Guid id, [FromBody] UpdateUserDto input)
        {
            return await _applicationService.UpdateAsync(id, input);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task DeleteAsync(Guid id)
        {
            await _applicationService.DeleteAsync(id);
        }
    }
}
