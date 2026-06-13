using Lucien.Domain.Shared.Interfaces;
using Lucien.Domain.Users.Entities;
using Lucien.Domain.Users.Interfaces;
using Lucien.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lucien.Infrastructure.Repositories.Users
{
    public class UserReposittory : Repository<User>, IUserRepository
    {
        private readonly LucienDbContext _context;
        private readonly IUserContextService _userContextService;
        private readonly ILogger<UserReposittory> _logger;


        public UserReposittory(LucienDbContext context, IUserContextService userContextService, ILogger<UserReposittory> logger) : base(userContextService, context, logger)
        {
            _context = context;
            _userContextService = userContextService;
            _logger = logger;
        }

        public async Task<User?> GetByEmailAsync(string? email)
        {
            if (email == null) throw new ArgumentNullException("email");

            return await GetQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Email != null && user.Email.ToLower() == email);
        }

        public override async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Id == id, cancellationToken)
                ?? throw new KeyNotFoundException($"User with Id {id} was not found.");
        }

        public override IQueryable<User> GetQueryable(bool includeSoftDeleted = false)
        {
            var query = _context.Users
                .Include(user => user.Role)
                .AsQueryable();

            if (includeSoftDeleted)
            {
                query = query.IgnoreQueryFilters();
            }

            return query;
        }
    }
}
