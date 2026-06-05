using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraLink.Domain.Entities;

namespace TerraLink.Domain.Persistence.Configurations;

/// <summary>
/// SQL Server configuration for AttachmentItemEntity
/// Table: AttachmentItems in Documents schema
/// </summary>
public class AttachmentItemEntityConfiguration : IEntityTypeConfiguration<AttachmentItemEntity>
{
    public void Configure(EntityTypeBuilder<AttachmentItemEntity> builder)
    {
        // Table name: AttachmentItems in Documents schema
        builder.ToTable("AttachmentItems", "Documents");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
               .HasMaxLength(255)
               .IsRequired();

        builder.ComplexProperty(a => a.Extension, a =>
        {
            a.Property(x => x.Value).HasMaxLength(50).HasColumnName("Extension").IsRequired();
        });

        builder.Property(a => a.MimeType)
               .HasMaxLength(200)
               .IsRequired();

        // Tracked properties from TrackedBaseEntity
        builder.Property(a => a.CreatedDate)
               .HasColumnName("CreatedAt")
               .HasColumnType("datetime2")
               .HasDefaultValueSql("SYSDATETIME()")
               .IsRequired();

        builder.Property(a => a.CreatedBy)
               .HasMaxLength(100);

        builder.Property(a => a.UpdatedDate)
               .HasColumnName("UpdatedAt")
               .HasColumnType("datetime2");

        builder.Property(a => a.ModifiedBy)
               .HasColumnName("UpdatedBy")
               .HasMaxLength(100);

        builder.HasOne(a => a.AttachmentType)
               .WithMany()
               .HasForeignKey(a => a.AttachmentTypeId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
