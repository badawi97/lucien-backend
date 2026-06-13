using Lucien.Domain.Roles.Entites;
using Lucien.Domain.Roles.Interfaces;
using Lucien.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lucien.Infrastructure.Repositories.Roles
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly LucienDbContext _context;
        private readonly IUserContextService _userContextService;
        private readonly ILogger<PermissionRepository> _logger;

        public PermissionRepository(LucienDbContext context, IUserContextService userContextService, ILogger<PermissionRepository> logger)
        {
            _context = context;
            _userContextService = userContextService;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<Permission>> GetByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            return await _context.Permissions
                .AsNoTracking()
                .Where(permission => EF.Property<Guid>(permission, "RoleId") == roleId)
                .OrderBy(permission => permission.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<Permission> CreateAsync(Guid roleId, Permission permission, string actor, CancellationToken cancellationToken = default)
        {
            var role = await _context.Roles
                .Include(role => role.Permissions)
                .FirstOrDefaultAsync(role => role.Id == roleId, cancellationToken)
                ?? throw new KeyNotFoundException($"Role with Id {roleId} was not found.");

            role.AddPermission(permission, actor);
            await _context.SaveChangesAsync(cancellationToken);

            return permission;
        }

        public async Task RemoveFromRoleAsync(Guid roleId, PermissionName name, string actor, CancellationToken cancellationToken = default)
        {
            var role = await _context.Roles
                .Include(role => role.Permissions)
                .FirstOrDefaultAsync(role => role.Id == roleId, cancellationToken)
                ?? throw new KeyNotFoundException($"Role with Id {roleId} was not found.");

            role.RemovePermission(name, actor);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
