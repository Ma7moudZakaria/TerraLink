using LowCodeHub.QueryableExtensions.ValueObjects.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraLink.Domain.Entities;
namespace TerraLink.Domain.Persistence.Configurations;

public class IncomingPaymentConfiguration : IEntityTypeConfiguration<IncomingPaymentEntity>
{
    public void Configure(EntityTypeBuilder<IncomingPaymentEntity> builder)
    {
        builder.ToTable("IncomingPayments", "Finance");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.ContractId).IsRequired();
        builder.Property(p => p.ClientId).IsRequired();
        builder.Property(p => p.SourceType).IsRequired();
        builder.Property(p => p.TransactionTypeId).IsRequired();
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

        builder.HasOne(p => p.Contract)
               .WithMany()
               .HasForeignKey(p => p.ContractId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(p => p.Client)
               .WithMany()
               .HasForeignKey(p => p.ClientId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(p => p.TransactionType)
               .WithMany()
               .HasForeignKey(p => p.TransactionTypeId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(p => p.PaymentMethod)
               .WithMany()
               .HasForeignKey(p => p.PaymentMethodId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(p => p.ContractInstallment)
              .WithMany(i => i.IncomingPayments)
              .HasForeignKey(p => p.ContractInstallmentId)
              .OnDelete(DeleteBehavior.NoAction);
    }
}

