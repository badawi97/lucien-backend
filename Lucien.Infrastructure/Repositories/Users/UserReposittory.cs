using Lucien.Domain.Shared.Interfaces;
using Lucien.Domain.Users.Entities;
using Lucien.Domain.Users.Interfaces;
using Lucien.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

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

            Expression<Func<User, bool>> predicate =
                user => user.Email != null &&
                        user.Email.ToLower() == email;

            return await _context.Users.FirstOrDefaultAsync(predicate);
        }
    }
}
