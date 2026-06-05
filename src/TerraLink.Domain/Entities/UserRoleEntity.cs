using Microsoft.AspNetCore.Identity;

namespace TerraLink.Domain.Entities
{
    public class UserRoleEntity : IdentityUserRole<Guid>
    {
        public UserEntity User { get; set; } = null!;
        public RoleEntity Role { get; set; } = null!;
    }
}
