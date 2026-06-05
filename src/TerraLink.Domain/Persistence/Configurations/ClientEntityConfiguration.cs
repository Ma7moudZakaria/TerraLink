using LowCodeHub.QueryableExtensions.ValueObjects.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraLink.Domain.Entities;

namespace TerraLink.Domain.Persistence.Configurations
{
    public class ClientEntityConfiguration : IEntityTypeConfiguration<ClientEntity>
    {
        public void Configure(EntityTypeBuilder<ClientEntity> builder)
        {
            builder.ToTable("Customers", "Sales");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(c => c.Code)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(c => c.Phone)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasMaxLength(255);

            builder.Property(c => c.NationalId)
                .HasMaxLength(50);

            builder.Property(c => c.Address)
                .HasMaxLength(500);

            builder.Property(c => c.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(c => c.Attachments)
               .HasColumnType("nvarchar(max)")
               .HasConversion<AttributesDictionaryConverter>();

            // Tracked properties from TrackedBaseEntity
            builder.Property(c => c.CreatedDate)
                .HasColumnName("CreatedAt")
                .HasColumnType("datetime2")
                .HasDefaultValueSql("SYSDATETIME()")
                .IsRequired();

            builder.Property(c => c.CreatedBy)
                .HasMaxLength(100);

            builder.Property(c => c.UpdatedDate)
                .HasColumnName("UpdatedAt")
                .HasColumnType("datetime2");

            builder.Property(c => c.ModifiedBy)
                .HasColumnName("UpdatedBy")
                .HasMaxLength(100);
        }
    }
}
