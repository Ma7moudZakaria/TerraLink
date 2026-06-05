namespace TerraLink.UseCase.Finance.Features.Installments.Operations;

public sealed partial class GetInstallmentByIdOperation
{
    public sealed class Response
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public string ContractCode { get; set; } = string.Empty;
        public Guid ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public Guid? UnitId { get; set; }
        public string? UnitCode { get; set; }
        public string? UnitName { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public string AmountText { get; set; } = string.Empty;
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
