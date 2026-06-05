using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.Domain.Entities
{
    public class ClientEntity : TrackedBaseEntity<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public AttributesDictionary? Attachments { get; set; }

        public ICollection<ClientContractEntity> ClientContracts { get; set; } = [];
    }
}
