using Lucien.Domain.Roles.Entites;
using Lucien.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Lucien.Infrastructure.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions", "identity");

            builder.HasKey(x => x.Id);

            // ValueObject via converter
            builder.Property(x => x.Name)
                .HasConversion(new PermissionNameConverter())
                .HasColumnName("Name")
                .HasMaxLength(200)
                .IsRequired();

            // Auditing
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt);
            builder.Property(x => x.CreatedBy).HasMaxLength(100);
            builder.Property(x => x.UpdatedBy).HasMaxLength(100);

            // Soft delete
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            builder.Property(x => x.DeletedAt);
            builder.Property(x => x.DeletedBy).HasMaxLength(100);

            // Shadow FK to Role
            builder.Property<Guid>("RoleId").IsRequired();
            builder.HasIndex("RoleId");
            builder.HasIndex(x => x.IsDeleted);
        }
    }
}
