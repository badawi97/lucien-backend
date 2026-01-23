using Lucien.Domain.Users.Entities;
using Lucien.Domain.Shared.Interfaces;
using Lucien.Domain.Users.Interfaces;
using Microsoft.EntityFrameworkCore;
using Lucien.Infrastructure.Repositories.Common;
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

        public async Task<User> GetByEmailAsync(string? email)
        {
            if (email == null) throw new ArgumentNullException("email");

            return await _context.Users.FirstOrDefaultAsync(user => user.Email == email)
                   ?? throw new KeyNotFoundException($"User with email {email} was not found.");
        }
    }
}
