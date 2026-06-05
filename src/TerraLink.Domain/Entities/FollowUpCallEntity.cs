using LowCodeHub.QueryableExtensions.Entities;

namespace TerraLink.Domain.Entities
{
    public class FollowUpCallEntity : TrackedBaseEntity<Guid>
    {
        public DateTime CallDate { get; set; }
        public required string CallerName { get; set; }
        public string? Note { get; set; }
        public bool IsDeleted { get; set; }

        public Guid ClientId { get; set; }
        public ClientEntity Client { get; set; } = default!;
    }
}
