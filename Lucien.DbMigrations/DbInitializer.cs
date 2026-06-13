using Lucien.Domain.Roles.Entites;
using Lucien.Domain.Shared.Authorization;
using Lucien.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Lucien.DbMigrations
{
    public class DbInitializer
    {
        private readonly LucienDbContext _dbContext;

        public DbInitializer(LucienDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            try
            {
                SeedRoles();
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine(dbEx.InnerException?.Message);
                throw;
            }
        }

        private void SeedRoles()
        {
            const string seedActor = "system";

            var rolePermissions = new Dictionary<string, IReadOnlyCollection<string>>
            {
                [RoleNames.Admin] = PermissionNames.All,
                [RoleNames.User] = new[]
                {
            PermissionNames.UsersRead,
            PermissionNames.CardsRead,
            PermissionNames.RolesRead,
            PermissionNames.PermissionsRead
        }
            };

            // 1. Fetch existing roles to memory 
            var existingRoles = _dbContext.Roles
                .IgnoreQueryFilters()
                .ToList();

            // 2. Fetch existing permissions to memory to evaluate duplicates safely
            // Note: If 'Name' maps to a Value Object in EF Core, access its primitive property (.Name.Value)
            var existingPermissions = _dbContext.Permissions
                .IgnoreQueryFilters()
                .Select(p => new { p.RoleId, NameValue = p.Name.Value })
                .ToHashSet();

            foreach (var roleSeed in rolePermissions)
            {
                var roleName = roleSeed.Key;

                // Find or create the role safely
                var role = existingRoles.FirstOrDefault(r => r.Name == roleName);
                if (role == null)
                {
                    role = new Role(roleName, seedActor);
                    _dbContext.Roles.Add(role);

                    // We must call SaveChanges here ONLY if the role is new 
                    // so that EF Core prompts the database to generate/populate the role.Id 
                    // before we map permissions to it.
                    _dbContext.SaveChanges();
                    existingRoles.Add(role);
                }

                // Add missing permissions for this specific role
                foreach (var permissionStr in roleSeed.Value)
                {
                    // Check if this specific Role already has this permission name string saved
                    var permissionExists = existingPermissions.Contains(new { RoleId = role.Id, NameValue = permissionStr });

                    if (!permissionExists)
                    {
                        // Create your strong-typed value object wrapper
                        var typedPermissionName = new PermissionName(permissionStr);

                        // Construct the permission entity 
                        var permission = new Permission(typedPermissionName, seedActor, role.Id);

                        _dbContext.Permissions.Add(permission);

                        // Track locally to prevent errors if duplicate items exist in the seeding dictionary
                        existingPermissions.Add(new { RoleId = role.Id, NameValue = permissionStr });
                    }
                }
            }

            // 3. Persist all clean, non-duplicate permissions safely
            _dbContext.SaveChanges();
        }

    }
}
