using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.Domain.Entities
{
    public class OutgoingPaymentEntity : TrackedBaseEntity<Guid>
    {
        public string Code { get; set; } = string.Empty;
        public required Guid ExpenseTypeId { get; set; }
        public required Guid BeneficiaryId { get; set; }
        public Guid? UnitId { get; set; }
        public Guid? BuildingId { get; set; }
        public required decimal Amount { get; set; }
        public required Guid PaymentMethodId { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Notes { get; set; } = string.Empty;
        public AttributesDictionary? Attachments { get; set; }

        public LookupItemEntity ExpenseType { get; set; } = default!;
        public LookupItemEntity Beneficiary { get; set; } = default!;
        public LookupItemEntity PaymentMethod { get; set; } = default!;
        public UnitEntity? Unit { get; set; }
        public BuildingEntity? Building { get; set; }
    }
}
