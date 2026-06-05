using LowCodeHub.QueryableExtensions.Entities;

namespace TerraLink.Domain.Entities
{
    public class ContractInstallmentEntity : TrackedBaseEntity<Guid>
    {
        public string Description { get; set; } = string.Empty;      // بيان الدفعة
        public DateTime DueDate { get; set; }         // تاريخ الاستحقاق
        public decimal Amount { get; set; }           // المبلغ بالأرقام
        public string AmountText { get; set; } = string.Empty;        // المبلغ بالحروف

        public bool IsDeleted { get; set; }

        public Guid ContractId { get; set; }
        public ContractEntity Contract { get; set; } = default!;
        public ICollection<IncomingPaymentEntity> IncomingPayments { get; set; } = [];
    }
}
