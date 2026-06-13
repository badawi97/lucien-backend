using Lucien.Domain.Roles.Entites;
using Lucien.Domain.Roles.Interfaces;
using Lucien.Domain.Shared.Interfaces;
using Lucien.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lucien.Infrastructure.Repositories.Roles
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly LucienDbContext _context;
        private readonly IUserContextService _userContextService;
        private readonly ILogger<RoleRepository> _logger;

        public RoleRepository(LucienDbContext context, IUserContextService userContextService, ILogger<RoleRepository> logger) : base(userContextService, context, logger)
        {
            _context = context;
            _userContextService = userContextService;
            _logger = logger;
        }

        public async Task<Role> RenameAsync(Guid id, string name, string actor, CancellationToken cancellationToken = default)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(role => role.Id == id, cancellationToken)
                ?? throw new KeyNotFoundException($"Role with Id {id} was not found.");

            role.Rename(name, actor);
            await _context.SaveChangesAsync(cancellationToken);

            return role;
        }
    }
}
