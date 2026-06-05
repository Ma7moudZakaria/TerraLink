using LowCodeHub.QueryableExtensions.ValueObjects.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraLink.Domain.Entities;

namespace TerraLink.Domain.Persistence.Configurations
{
    internal class ContractEntityConfiguration : IEntityTypeConfiguration<ContractEntity>
    {
        public void Configure(EntityTypeBuilder<ContractEntity> builder)
        {
            builder.ToTable("Contracts", "Sales");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.ContractDate)
                   .HasColumnType("datetime2")
                   .IsRequired();

            builder.Property(c => c.TotalPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(c => c.Notes)
                   .HasMaxLength(2000);

            builder.Property(c => c.IsInstallmentPlan)
                   .HasDefaultValue(false)
                   .IsRequired();

            builder.Property(c => c.IsDeleted)
                   .HasDefaultValue(false)
                   .IsRequired();

            builder.Property(c => c.Attachments)
                   .HasColumnType("nvarchar(max)")
                   .HasConversion<AttributesDictionaryConverter>();

            builder.HasOne(c => c.PaymentMethod)
                   .WithMany()
                   .HasForeignKey(c => c.PaymentMethodId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Client)
                   .WithMany()
                   .HasForeignKey(c => c.ClientId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Land)
                   .WithMany()
                   .HasForeignKey(c => c.LandId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Building)
                   .WithMany()
                   .HasForeignKey(c => c.BuildingId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Unit)
                   .WithMany(u => u.Contracts)
                   .HasForeignKey(c => c.UnitId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Property(c => c.UnitPriceAtContract)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

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

            builder.HasMany(c => c.Installments)
                   .WithOne(i => i.Contract)
                   .HasForeignKey(i => i.ContractId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.User)
                   .WithMany()
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            // Soft Delete default
            builder.Property(c => c.IsDeleted).HasDefaultValue(false);
        }
    }
}
