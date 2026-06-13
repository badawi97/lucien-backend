using Lucien.Domain.Sessions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lucien.Infrastructure.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("Sessions", "identity");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.RefreshToken).HasMaxLength(500);
            builder.Property(x => x.ExpiresAt).IsRequired();

            // Auditing
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt);
            builder.Property(x => x.CreatedBy).HasMaxLength(100);
            builder.Property(x => x.UpdatedBy).HasMaxLength(100);

            // Soft delete
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            builder.Property(x => x.DeletedAt);
            builder.Property(x => x.DeletedBy).HasMaxLength(100);

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.RefreshToken);
            builder.HasIndex(x => x.ExpiresAt);
            builder.HasIndex(x => x.IsDeleted);
        }
    }
}
