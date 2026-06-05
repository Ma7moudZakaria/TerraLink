using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.ValueObjects.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TerraLink.Domain.Persistence.Configurations;

public class LookupSetEntityConfiguration : IEntityTypeConfiguration<LookupSetEntity>
{
    public void Configure(EntityTypeBuilder<LookupSetEntity> builder)
    {
        builder.ToTable("LookupSets", "Lookups");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Code)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(l => l.Descriptions)
               .HasColumnType("nvarchar(max)")
               .HasConversion<LocalizedConverter>();

        builder.Property(l => l.IsActive)
               .HasDefaultValue(true);

        // Tracked properties from TrackedBaseEntity
        builder.Property(l => l.CreatedDate)
               .HasColumnName("CreatedAt")
               .HasColumnType("datetime2")
               .HasDefaultValueSql("SYSDATETIME()")
               .IsRequired();

        builder.Property(l => l.CreatedBy)
               .HasMaxLength(100);

        builder.Property(l => l.UpdatedDate)
               .HasColumnName("UpdatedAt")
               .HasColumnType("datetime2");

        builder.Property(l => l.ModifiedBy)
               .HasColumnName("UpdatedBy")
               .HasMaxLength(100);

        builder.HasMany(l => l.LookupItems)
               .WithOne(i => i.LookupSet)
               .HasForeignKey(i => i.LookupSetId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
