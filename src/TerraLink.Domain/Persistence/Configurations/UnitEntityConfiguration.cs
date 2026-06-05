using LowCodeHub.QueryableExtensions.ValueObjects.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraLink.Domain.Entities;

namespace TerraLink.Domain.Persistence.Configurations;

public class UnitEntityConfiguration : IEntityTypeConfiguration<UnitEntity>
{
    public void Configure(EntityTypeBuilder<UnitEntity> builder)
    {
        builder.ToTable("Units", "Assets");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(u => u.Number)
     .HasMaxLength(100)
   .IsRequired();

        builder.Property(u => u.FloorNumber)
   .IsRequired();

        builder.Property(u => u.Area)
      .HasColumnType("decimal(18,2)")
    .IsRequired();

        builder.Property(u => u.NumberOfRooms)
   .IsRequired();

        builder.Property(u => u.NumberOfBathrooms)
           .IsRequired();

        builder.Property(u => u.Price)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(u => u.Description)
      .HasMaxLength(2000);

        builder.Property(u => u.IsDeleted)
         .HasDefaultValue(false);

        builder.Property(u => u.Attachments)
  .HasColumnType("nvarchar(max)")
               .HasConversion<AttributesDictionaryConverter>();

        builder.Property(u => u.CreatedDate)
 .HasColumnName("CreatedAt")
   .HasColumnType("datetime2")
            .HasDefaultValueSql("SYSDATETIME()")
       .IsRequired();

        builder.Property(u => u.CreatedBy)
        .HasMaxLength(100);

        builder.Property(u => u.UpdatedDate)
             .HasColumnName("UpdatedAt")
       .HasColumnType("datetime2");

        builder.Property(u => u.ModifiedBy)
        .HasColumnName("UpdatedBy")
   .HasMaxLength(100);

        // Relationships
        builder.HasOne(u => u.Building)
               .WithMany(a => a.Units)
               .HasForeignKey(u => u.BuildingId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(u => u.UnitType)
               .WithMany()
               .HasForeignKey(u => u.UnitTypeId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(u => u.UnitStatus)
               .WithMany()
               .HasForeignKey(u => u.UnitStatusId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(u => u.FinishingType)
               .WithMany()
               .HasForeignKey(u => u.FinishingTypeId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
