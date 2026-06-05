namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Operations;

public sealed partial class GetContractInstallmentsPaymentStatusOperation
{
    public sealed class Response
    {
        public Guid InstallmentId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public string AmountText { get; set; } = string.Empty;
        public bool IsPaid { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
