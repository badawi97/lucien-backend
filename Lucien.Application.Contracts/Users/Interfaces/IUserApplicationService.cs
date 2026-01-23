using Lucien.Domain.Shared.DI;
using Lucien.Application.Contracts.Users.Dtos;
using Lucien.Application.Contracts.Common.Dto;

namespace Lucien.Application.Contracts.Users.Interfaces
{
    public interface IUserApplicationService : ITransient
    {
        Task<UserDto> GetAsync(Guid id);
        Task<UserDto> GetByEmailAsync(string? email);
        Task<PagedResultDto<UserDto>> GetListAsync(PagedRequestUserDto input);
        Task<UserDto> CreateAsync(CreateUserDto input);
        Task<UserDto> UpdateAsync(Guid id, UpdateUserDto input);
        Task DeleteAsync(Guid id);
    }
}
