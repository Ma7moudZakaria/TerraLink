using Microsoft.AspNetCore.Identity;

namespace TerraLink.Domain.Entities
{
    public class RoleEntity : IdentityRole<Guid>
    {
        public string? Description { get; set; }
        public ICollection<UserRoleEntity> UserRoles { get; set; } = [];
        public ICollection<RolePermissionEntity> RolePermissions { get; set; } = [];
    }
}
