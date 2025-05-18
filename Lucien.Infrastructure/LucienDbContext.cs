using Lucien.Domain.Cards.Entities;
using Microsoft.EntityFrameworkCore;
using Lucien.Domain.Shared.Entities;
using Lucien.Domain.Shared.Interfaces;
using Lucien.Domain.Users.Entities;
using Lucien.Domain.Sessions.Entities;

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

        public DbSet<Card> Cards { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var currentUserId = _userContextService.GetCurrentUserId();

            // Handle auditing for entities that implement AuditableEntity
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
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
            // Apply global filters and configurations for soft delete and other entities
            modelBuilder.Entity<Card>()
                .HasQueryFilter(e => !e.IsDeleted)
                .ToTable("Cards");
            modelBuilder.Entity<User>()
                .HasQueryFilter(e => !e.IsDeleted)
                .ToTable("Users", "identity");
            modelBuilder.Entity<Session>()
                .HasQueryFilter(e => !e.IsDeleted)
                .ToTable("Sessions", "identity");

            // Other entities can be configured here as needed
            // Example: modelBuilder.Entity<OtherEntity>().HasQueryFilter(e => !e.IsDeleted);

            base.OnModelCreating(modelBuilder);
        }
    }
}
