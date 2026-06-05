using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraLink.Domain.Entities;

namespace TerraLink.Domain.Persistence.Configurations
{
    public class FollowUpCallEntityConfiguration : IEntityTypeConfiguration<FollowUpCallEntity>
    {
        public void Configure(EntityTypeBuilder<FollowUpCallEntity> builder)
        {
            builder.ToTable("FollowUpCalls", "Sales");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CallDate)
                   .HasColumnType("datetime2")
                   .IsRequired();

            builder.Property(x => x.CallerName)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(x => x.Note)
                   .HasMaxLength(2000);

            builder.Property(x => x.IsDeleted)
                   .HasColumnType("bit")
                   .HasDefaultValue(false)
                   .IsRequired();

            builder.HasOne(x => x.Client)
                   .WithMany()
                   .HasForeignKey(x => x.ClientId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Property(x => x.CreatedDate)
                   .HasColumnName("CreatedAt")
                   .HasColumnType("datetime2")
                   .HasDefaultValueSql("SYSDATETIME()")
                   .IsRequired();

            builder.Property(x => x.CreatedBy)
                   .HasMaxLength(100);

            builder.Property(x => x.UpdatedDate)
                   .HasColumnName("UpdatedAt")
                   .HasColumnType("datetime2");

            builder.Property(x => x.ModifiedBy)
                   .HasColumnName("UpdatedBy")
                   .HasMaxLength(100);

            builder.HasIndex(x => x.CallDate);
            builder.HasIndex(x => x.ClientId);
            builder.HasIndex(x => x.IsDeleted);
        }
    }
}
