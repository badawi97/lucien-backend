using Lucien.Domain.Cards.Entities;
using Microsoft.EntityFrameworkCore;
using Lucien.Domain.Shared.Entities;
using Lucien.Domain.Shared.Interfaces;
using Lucien.Domain.Users.Entities;
using Lucien.Domain.Sessions.Entities;
using Lucien.Infrastructure.Configurations;
using Lucien.Domain.Roles.Entites;

namespace Lucien.Infrastructure
{
    public class LucienDbContext : DbContext
    {
        private readonly IUserContextService _userContextService;

        public LucienDbContext(DbContextOptions<LucienDbContext> options, IUserContextService userContextService)
            : base(options)
        {
            _userContextService = userContextService;
        }

        // Define DbSet properties for entities 
        // add Global query filters for soft delete into OnModelCreating
        // Apply configurations and schema for entities 
        public DbSet<Card> Cards => Set<Card>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Session> Sessions => Set<Session>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Permission> Permissions => Set<Permission>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var currentUserId = _userContextService.GetCurrentUserId();

            // Handle auditing for entities that implement AuditableEntityBase
            foreach (var entry in ChangeTracker.Entries<AuditableEntityBase>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.SetAuditOnCreate(currentUserId);
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.SetAuditOnUpdate(currentUserId);
                }
                else if (entry.State == EntityState.Deleted)
                {
                    // Soft delete: mark as deleted and avoid actual deletion from DB
                    entry.State = EntityState.Modified;
                    entry.Entity.SoftDelete(currentUserId);

                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply schema and table name for entities 
            modelBuilder.Entity<Card>().ToTable("Cards");
            modelBuilder.Entity<User>().ToTable("Users", "identity");
            modelBuilder.Entity<Session>().ToTable("Sessions", "identity");

            // Apply entities configurations
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());

            // Global query filters for soft delete
            modelBuilder.Entity<Card>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Session>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Role>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Permission>().HasQueryFilter(e => !e.IsDeleted);

            base.OnModelCreating(modelBuilder);
        }
    }
}
