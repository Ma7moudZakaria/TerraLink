using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraLink.Domain.Entities;

namespace TerraLink.Domain.Persistence.Configurations
{
    public class ContractInstallmentEntityConfiguration : IEntityTypeConfiguration<ContractInstallmentEntity>
    {
        public void Configure(EntityTypeBuilder<ContractInstallmentEntity> builder)
        {
            builder.ToTable("ContractInstallments", "Sales");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Description)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(i => i.DueDate)
                   .HasColumnType("datetime2")
                   .IsRequired();

            builder.Property(i => i.Amount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(i => i.AmountText)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(i => i.IsDeleted)
                   .HasDefaultValue(false)
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

            // =========================
            // Relationship
            // =========================
            builder.HasOne(i => i.Contract)
                   .WithMany(c => c.Installments)
                   .HasForeignKey(i => i.ContractId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(i => i.IsDeleted).HasDefaultValue(false);
        }
    }
}
