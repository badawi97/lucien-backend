using Lucien.Domain.Roles.Entites;
using Lucien.Domain.Roles.Interfaces;
using Lucien.Domain.Shared.Interfaces;
using Lucien.Infrastructure.Repositories.Common;
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
    }
}
