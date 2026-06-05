using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraLink.Domain.Entities;

namespace TerraLink.Domain.Persistence.Configurations;

public class RoleEntityConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.ToTable("Roles", "IdentityShield");

        // Description property
        builder.Property(r => r.Description)
            .HasColumnType("nvarchar(max)");
    }
}
