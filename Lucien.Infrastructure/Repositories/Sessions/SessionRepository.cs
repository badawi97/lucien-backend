using Lucien.Domain.Sessions.Entities;
using Lucien.Domain.Sessions.Interfaces;
using Lucien.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lucien.Infrastructure.Repositories.Sessions
{
    public class SessionRepository : ISessionRepository
    {
        private readonly LucienDbContext _context;
        private readonly IUserContextService _userContextService;
        public SessionRepository(LucienDbContext context, IUserContextService userContextService)
        {
            _context = context;
            _userContextService = userContextService;
        }

        public async Task<Session> CreateAsync(Session session)
        {
            try
            {
                _context.Sessions.Add(session);
                await _context.SaveChangesAsync();
                return session;
            }
            catch (Exception ex)
            {
                throw new Exception("Not created");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var session = await GetByIdAsync(id);
                _context.Sessions.Remove(session);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Session> GetByIdAsync(Guid id)
        {
            return await _context.Sessions.FindAsync(id)
                               ?? throw new KeyNotFoundException($"Session with Id {id} was not found.");
        }

        public async Task<Session> GetByRefreshTokenAsync(string? refreshToken)
        {
            return await _context.Sessions.FirstOrDefaultAsync(session => session.RefreshToken == refreshToken)
                                           ?? throw new KeyNotFoundException($"Session was not found.");
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Sessions.CountAsync();
        }

        public Task<List<Session>> GetListAsync(string? name, DateTime? DateOfBirth, string? phone, int? gender, string? email, int skipCount, int maxResultCount, string? sorting)
        {
            throw new NotImplementedException();
        }

        public Task<Session> UpdateAsync(Guid id, Session card)
        {
            throw new NotImplementedException();
        }
    }
}
