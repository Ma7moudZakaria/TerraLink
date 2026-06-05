using LowCodeHub.QueryableExtensions.ValueObjects.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraLink.Domain.Entities;

namespace TerraLink.Domain.Persistence.Configurations;

public class OutgoingPaymentConfiguration : IEntityTypeConfiguration<OutgoingPaymentEntity>
{
    public void Configure(EntityTypeBuilder<OutgoingPaymentEntity> builder)
    {
        builder.ToTable("OutgoingPayments", "Finance");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.ExpenseTypeId).IsRequired();
        builder.Property(p => p.BeneficiaryId).IsRequired();
        builder.Property(p => p.PaymentMethodId).IsRequired();

        builder.Property(p => p.Code)
              .HasMaxLength(200)
              .IsRequired();

        builder.HasIndex(p => p.Code).IsUnique();

        builder.Property(p => p.Amount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(p => p.PaymentDate)
               .HasColumnType("datetime2");

        builder.Property(p => p.Notes)
               .HasColumnType("nvarchar(max)");

        builder.Property(b => b.IsDeleted)
              .HasDefaultValue(false);

        builder.Property(p => p.Attachments)
               .HasColumnType("nvarchar(max)")
               .HasConversion<AttributesDictionaryConverter>();

        builder.Property(p => p.CreatedDate)
               .HasColumnName("CreatedAt")
               .HasColumnType("datetime2")
               .HasDefaultValueSql("SYSDATETIME()")
               .IsRequired();

        builder.Property(p => p.CreatedBy)
               .HasMaxLength(100);

        builder.Property(p => p.UpdatedDate)
               .HasColumnName("UpdatedAt")
               .HasColumnType("datetime2");

        builder.Property(p => p.ModifiedBy)
               .HasColumnName("UpdatedBy")
               .HasMaxLength(100);

        builder.HasOne(p => p.ExpenseType)
               .WithMany()
               .HasForeignKey(p => p.ExpenseTypeId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(p => p.Beneficiary)
               .WithMany()
               .HasForeignKey(p => p.BeneficiaryId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(p => p.PaymentMethod)
               .WithMany()
               .HasForeignKey(p => p.PaymentMethodId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(p => p.Unit)
               .WithMany()
               .HasForeignKey(p => p.UnitId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(p => p.Building)
               .WithMany()
               .HasForeignKey(p => p.BuildingId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
