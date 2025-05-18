using Lucien.Domain.Shared.DI;
using Lucien.Application.Contracts.Token.Dtos;
using Lucien.Application.Contracts.Users.Dtos;

namespace Lucien.Application.Contracts.Token.Interfaces
{
    public interface ITokenApplicationService : ITransient
    {
        Task<TokenDto> GetAsync(UserDto user);
    }
}
