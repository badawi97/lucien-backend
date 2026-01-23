using Lucien.Domain.Sessions.Entities;
using Lucien.Domain.Sessions.Interfaces;
using Lucien.Domain.Shared.Interfaces;
using Lucien.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lucien.Infrastructure.Repositories.Sessions
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        private readonly LucienDbContext _context;
        private readonly IUserContextService _userContextService;
        private readonly ILogger<SessionRepository> _logger;

        public SessionRepository(LucienDbContext context, IUserContextService userContextService, ILogger<SessionRepository> logger) : base(userContextService, context, logger)
        {
            _context = context;
            _userContextService = userContextService;
            _logger = logger;
        }

        public async Task<Session> GetByRefreshTokenAsync(string? refreshToken)
        {
            return await _context.Sessions.FirstOrDefaultAsync(session => session.RefreshToken == refreshToken)
                                           ?? throw new KeyNotFoundException($"Session was not found.");
        }
    }
}
