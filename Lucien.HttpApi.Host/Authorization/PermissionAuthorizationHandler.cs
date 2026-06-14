using System.Security.Claims;
using Lucien.Domain.Roles.Entites;
using Lucien.Domain.Shared.Settings;
using Lucien.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Lucien.HttpApi.Host.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly LucienDbContext _context;

        public PermissionAuthorizationHandler(LucienDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var roleName = context.User.FindFirst(JwtClaimConst.Role)?.Value
                ?? context.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrWhiteSpace(roleName))
            {
                return;
            }

            var role = await _context.Roles
                .Include(role => role.Permissions)
                .AsNoTracking()
                .FirstOrDefaultAsync(role => role.Name == roleName);

            if (role == null)
            {
                return;
            }

            if (role.Permissions.Any(permission => permission.Name.Equals(new PermissionName(requirement.Permission))))
            {
                context.Succeed(requirement);
            }
        }
    }
}
