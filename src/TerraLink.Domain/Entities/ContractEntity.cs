using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.Domain.Entities
{
    public class ContractEntity : TrackedBaseEntity<Guid>
    {
        public string ContractNumber { get; set; } = string.Empty;
        public DateTime ContractDate { get; set; }          // تاريخ العقد
        public decimal TotalPrice { get; set; }              // قيمة العقد
        public string? Notes { get; set; }

        public Guid PaymentMethodId { get; set; }             // بيع / إيجار (Lookup)
        public LookupItemEntity PaymentMethod { get; set; } = default!;

        public Guid ClientId { get; set; }                   // Dropdown
        public ClientEntity Client { get; set; } = default!;

        public Guid? LandId { get; set; }
        public LandEntity? Land { get; set; }

        public Guid? BuildingId { get; set; }
        public BuildingEntity? Building { get; set; }

        public Guid? UnitId { get; set; }
        public UnitEntity? Unit { get; set; }

        public Guid UserId { get; set; }
        public UserEntity User { get; set; } = default!;

        public decimal UnitPriceAtContract { get; set; }     // السعر وقت التعاقد

        public bool IsInstallmentPlan { get; set; }

        public AttributesDictionary? Attachments { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<ClientContractEntity> ClientContracts { get; set; } = [];
        public ICollection<ContractInstallmentEntity> Installments { get; set; } = [];
    }
}
