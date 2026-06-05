using LowCodeHub.QueryableExtensions.Entities;

namespace TerraLink.Domain.Entities
{
    /// <summary>
    /// Represents the many-to-many relationship between roles and permissions (stored as lookup items)
    /// </summary>
    public class RolePermissionEntity : BaseEntity<Guid>
    {
        public Guid RoleId { get; set; }
        public RoleEntity Role { get; set; } = default!;

        /// <summary>
        /// Foreign key to LookupItemEntity representing the permission
        /// </summary>
        public Guid PermissionLookupItemId { get; set; }

        /// <summary>
        /// Navigation property to the permission lookup item
        /// </summary>
        public LookupItemEntity PermissionLookupItem { get; set; } = default!;
    }
}
