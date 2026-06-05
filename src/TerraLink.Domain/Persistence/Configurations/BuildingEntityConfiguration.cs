using LowCodeHub.QueryableExtensions.ValueObjects.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraLink.Domain.Entities;
namespace TerraLink.Domain.Persistence.Configurations;

public class BuildingEntityConfiguration : IEntityTypeConfiguration<BuildingEntity>
{
    public void Configure(EntityTypeBuilder<BuildingEntity> builder)
    {
        builder.ToTable("Buildings", "Assets");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(b => b.Number)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(b => b.NumberOfFloors).IsRequired();

        builder.Property(b => b.NumberOfUnits).IsRequired();

        builder.Property(b => b.ConstructionYear)
               .HasColumnType("datetime2");

        builder.Property(b => b.Length)
               .HasColumnType("decimal(18,2)").IsRequired();

        builder.Property(b => b.Width)
               .HasColumnType("decimal(18,2)").IsRequired();

        builder.Property(b => b.Description)
               .HasMaxLength(2000);

        builder.Property(b => b.IsDeleted)
                .HasDefaultValue(false);

        builder.Property(b => b.Attachments)
               .HasColumnType("nvarchar(max)")
               .HasConversion<AttributesDictionaryConverter>();

        builder.HasMany(b => b.Units)
               .WithOne(u => u.Building)
               .HasForeignKey(u => u.BuildingId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.Property(b => b.CreatedDate)
               .HasColumnName("CreatedAt")
               .HasColumnType("datetime2")
               .HasDefaultValueSql("SYSDATETIME()")
               .IsRequired();

        builder.Property(b => b.CreatedBy)
               .HasMaxLength(100);

        builder.Property(b => b.UpdatedDate)
               .HasColumnName("UpdatedAt")
               .HasColumnType("datetime2");

        builder.Property(b => b.ModifiedBy)
               .HasColumnName("UpdatedBy")
               .HasMaxLength(100);

        builder.HasOne(b => b.Land)
               .WithMany(a => a.Buildings)
               .HasForeignKey(b => b.LandId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.BuildingStatus)
               .WithMany()
               .HasForeignKey(b => b.BuildingStatusId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
