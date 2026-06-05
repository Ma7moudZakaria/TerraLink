using LowCodeHub.QueryableExtensions.ValueObjects.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraLink.Domain.Entities;
namespace TerraLink.Domain.Persistence.Configurations;

public class LandEntityConfiguration : IEntityTypeConfiguration<LandEntity>
{
    public void Configure(EntityTypeBuilder<LandEntity> builder)
    {
        builder.ToTable("Lands", "Assets");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Name)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(l => l.Number)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(l => l.Length)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(l => l.Width)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(l => l.Latitude)
            .HasColumnType("decimal(10,7)");

        builder.Property(l => l.Longitude)
            .HasColumnType("decimal(10,7)");

        builder.Property(l => l.Description)
            .HasMaxLength(2000);

        builder.Property(l => l.Attachments)
               .HasColumnType("nvarchar(max)")
               .HasConversion<AttributesDictionaryConverter>();

        builder.Property(l => l.IsDeleted)
                   .HasColumnType("bit")
                   .HasDefaultValue(false)
                   .IsRequired();

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

        builder.HasOne(l => l.Governorate)
               .WithMany()
               .HasForeignKey(l => l.GovernorateId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(l => l.City)
               .WithMany()
               .HasForeignKey(l => l.CityId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(l => l.District)
               .WithMany()
               .HasForeignKey(l => l.DistrictId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(l => l.Buildings)
               .WithOne(b => b.Land)
               .HasForeignKey(b => b.LandId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
