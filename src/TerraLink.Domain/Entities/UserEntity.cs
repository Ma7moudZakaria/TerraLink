using LowCodeHub.QueryableExtensions.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace TerraLink.Domain.Entities
{
    public class UserEntity : IdentityUser<Guid>
    {
        public required string Name { get; set; }
        public string? Phone { get; set; }
        public bool? IsActive { get; set; }
        public AttributeDictionary? Attributes { get; set; }
        public ICollection<UserRoleEntity> UserRoles { get; set; } = [];
    }
}
