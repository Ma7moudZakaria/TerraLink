using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraLink.Domain.Entities;

namespace TerraLink.Domain.Persistence.Configurations
{
    public class ClientContractEntityConfiguration : IEntityTypeConfiguration<ClientContractEntity>
    {
        public void Configure(EntityTypeBuilder<ClientContractEntity> builder)
        {
            builder.ToTable("CustomerContracts", "Sales");

            builder.HasKey(p => p.Id);

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

            //builder.HasOne(p => p.Client)
            //    .WithMany(c => c.ClientContracts)
            //    .HasForeignKey(p => p.ClientId)
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Contract)
                   .WithMany(c => c.ClientContracts)
                   .HasForeignKey(i => i.ContractId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(i => i.IsDeleted).HasDefaultValue(false);
        }
    }
}
