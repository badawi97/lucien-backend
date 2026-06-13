using Lucien.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lucien.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "identity");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName).HasMaxLength(100);
            builder.Property(x => x.LastName).HasMaxLength(100);
            builder.Property(x => x.Email).HasMaxLength(200);
            builder.Property(x => x.Phone).HasMaxLength(50);
            builder.Property(x => x.Photo).HasMaxLength(500);
            builder.Property(x => x.Address).HasMaxLength(500);
            builder.Property(x => x.PasswordHash).HasMaxLength(500);

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
            builder.HasIndex(x => x.Email);
            builder.HasIndex(x => x.Phone);
            builder.HasIndex(x => x.IsDeleted);
        }
    }
}
