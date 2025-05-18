using Lucien.Application.Contracts.Auth.Dtos;
using Lucien.Application.Contracts.Token.Dtos;
using Lucien.Domain.Shared.DI;

namespace Lucien.Application.Contracts.Auth.Interfaces
{
    public interface IAuthApplicationService : ITransient
    {
        Task<TokenDto> LoginAsync(LoginDto loginDto);
        Task<TokenDto> GetRefreshTokenAsync(string? refreshToken);
    }
}
