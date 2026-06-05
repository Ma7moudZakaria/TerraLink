using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.ValueObjects.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TerraLink.Domain.Persistence.Configurations;

public class LookupItemEntityConfiguration : IEntityTypeConfiguration<LookupItemEntity>
{
    public void Configure(EntityTypeBuilder<LookupItemEntity> builder)
    {
        builder.ToTable("LookupItems", "Lookups");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Code)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(l => l.Descriptions)
               .HasColumnType("nvarchar(max)")
               .HasConversion<LocalizedConverter>();

        builder.Property(l => l.Metadata)
               .HasColumnType("nvarchar(max)")
               .HasConversion<AttributeDictionaryConverter>();

        builder.Property(l => l.SortOrder)
               .HasDefaultValue(0);

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

        builder.HasOne(l => l.LookupSet)
               .WithMany(s => s.LookupItems)
               .HasForeignKey(l => l.LookupSetId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
