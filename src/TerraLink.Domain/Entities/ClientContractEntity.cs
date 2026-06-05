using LowCodeHub.QueryableExtensions.Entities;

namespace TerraLink.Domain.Entities
{
    public class ClientContractEntity : TrackedBaseEntity<Guid>
    {
        public required Guid ClientId { get; set; }
        public ClientEntity Client { get; set; } = default!;

        public required Guid ContractId { get; set; }
        public ContractEntity Contract { get; set; } = default!;

        public bool IsDeleted { get; set; }
    }
}
