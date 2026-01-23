using Lucien.Domain.Comman.Interfaces;
using Lucien.Domain.Sessions.Entities;
using Lucien.Domain.Shared.DI;

namespace Lucien.Domain.Sessions.Interfaces
{
    public interface ISessionRepository : IRepository<Session>, ITransient
    {
        Task<Session> GetByRefreshTokenAsync(string? refreshToken);
    }
}
