using Lucien.Application.Contracts.Sessions.Dto;
using Lucien.Domain.Shared.DI;

namespace Lucien.Application.Contracts.Sessions.Interfaces
{
    public interface ISessionApplicationService : ITransient
    {
        Task<SessionDto> GetByRefreshTokenAsync(string? refreshToken);
        Task<bool> ValidateRefreshTokenAsync(Guid userId, string? refreshToken);
        Task<SessionDto> CreateAsync(CreateSessionDto input);

    }
}
