using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.ValueObjects;
using TerraLink.Domain.Enums;

namespace TerraLink.Domain.Entities
{
    public class IncomingPaymentEntity : TrackedBaseEntity<Guid>
    {
        public string Code { get; set; } = string.Empty;
        public required Guid ContractId { get; set; }
        public Guid? ContractInstallmentId { get; set; }
        public required Guid ClientId { get; set; }
        public IncomingPaymentSourceType SourceType { get; set; }
        public required Guid TransactionTypeId { get; set; }
        public required decimal Amount { get; set; }
        public required Guid PaymentMethodId { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Notes { get; set; } = string.Empty;
        public AttributesDictionary? Attachments { get; set; }

        public ClientEntity Client { get; set; } = default!;
        public ContractEntity Contract { get; set; } = default!;
        public LookupItemEntity TransactionType { get; set; } = default!;
        public LookupItemEntity PaymentMethod { get; set; } = default!;
        public ContractInstallmentEntity? ContractInstallment { get; set; }
    }
}
