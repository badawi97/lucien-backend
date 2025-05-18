using Lucien.Domain.Sessions.Entities;
using Lucien.Domain.Shared.DI;

namespace Lucien.Domain.Sessions.Interfaces
{
    public interface ISessionRepository : ITransient
    {
        Task<List<Session>> GetListAsync(
            string? name,
            DateTime? DateOfBirth,
            string? phone,
            int? gender,
            string? email,
            int skipCount,
            int maxResultCount,
            string? sorting
            );

        Task<Session> GetByIdAsync(Guid id);
        Task<Session> GetByRefreshTokenAsync(string? refreshToken);
        Task<Session> CreateAsync(Session card);
        Task<Session> UpdateAsync(Guid id, Session card);
        Task DeleteAsync(Guid id);
        Task<int> GetCountAsync();
    }
}
