using LowCodeHub.QueryableExtensions.Entities;

namespace TerraLink.Domain.Entities
{
    public class RefreshTokenEntity : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public UserEntity User { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? RevokedAt { get; set; }
    }
}
