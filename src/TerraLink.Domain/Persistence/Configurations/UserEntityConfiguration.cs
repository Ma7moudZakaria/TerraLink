using LowCodeHub.QueryableExtensions.ValueObjects.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraLink.Domain.Entities;

namespace TerraLink.Domain.Persistence.Configurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users", "IdentityShield");

        // Custom properties
        builder.Property(u => u.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(u => u.Phone)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(u => u.IsActive)
            .HasDefaultValue(true);

        // Attributes property - stored as JSON
        builder.Property(u => u.Attributes)
            .HasColumnType("NVARCHAR(MAX)")
            .HasConversion<AttributeDictionaryConverter>()
            .IsRequired(false);
    }
}
