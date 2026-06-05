using LowCodeHub.QueryableExtensions.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TerraLink.Domain.Entities;

namespace TerraLink.Domain.Persistence;

public class TerraLinkDbContext(DbContextOptions<TerraLinkDbContext> options) : IdentityDbContext<UserEntity, RoleEntity, Guid, Microsoft.AspNetCore.Identity.IdentityUserClaim<Guid>, UserRoleEntity, Microsoft.AspNetCore.Identity.IdentityUserLogin<Guid>, Microsoft.AspNetCore.Identity.IdentityRoleClaim<Guid>, Microsoft.AspNetCore.Identity.IdentityUserToken<Guid>>(options)
{
    public DbSet<LandEntity> Lands => Set<LandEntity>();
    public DbSet<BuildingEntity> Buildings => Set<BuildingEntity>();
    public DbSet<UnitEntity> Units => Set<UnitEntity>();
    public DbSet<AttachmentItemEntity> AttachmentItems => Set<AttachmentItemEntity>();
    public DbSet<LookupSetEntity> LookupSets => Set<LookupSetEntity>();
    public DbSet<LookupItemEntity> LookupItems => Set<LookupItemEntity>();
    public DbSet<ClientEntity> Clients => Set<ClientEntity>();
    public DbSet<RolePermissionEntity> RolePermissions => Set<RolePermissionEntity>();
    public DbSet<RefreshTokenEntity> RefreshTokens => Set<RefreshTokenEntity>();
    public DbSet<ContractEntity> Contracts => Set<ContractEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<Guid>>().ToTable("UserClaims", "IdentityShield");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<Guid>>().ToTable("UserLogins", "IdentityShield");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<Guid>>().ToTable("UserTokens", "IdentityShield");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<Guid>>().ToTable("RoleClaims", "IdentityShield");

        builder.ApplyConfigurationsFromAssembly(typeof(IApplicationDbTypeConfigurationMarker).Assembly, t => t.Namespace?.Contains("Persistence.Configurations") == true);
    }
}
