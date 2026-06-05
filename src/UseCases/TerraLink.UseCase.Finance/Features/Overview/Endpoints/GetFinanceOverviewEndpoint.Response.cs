namespace TerraLink.UseCase.Finance.Features.Overview.Endpoints;

public sealed partial class GetFinanceOverviewEndpoint
{
    public sealed class MonthlyInstallmentsPointResponse
    {
        public int Month { get; set; }
        public decimal DueAmount { get; set; }
        public decimal PaidAmount { get; set; }
    }

    public sealed class UrgentActionResponse
    {
        public Guid ContractId { get; set; }
        public string ContractNumber { get; set; } = string.Empty;
        public Guid InstallmentId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public decimal RemainingAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public sealed class Response
    {
        public decimal IncomingPaymentsThisMonth { get; set; }
        public decimal IncomingPaymentsThisYear { get; set; }
        public decimal DueInstallmentsThisMonth { get; set; }
        public decimal PaidInstallmentsThisMonth { get; set; }
        public decimal OverdueInstallmentsAmount { get; set; }
        public decimal CollectionRatePercentage { get; set; }
        public List<MonthlyInstallmentsPointResponse> MonthlyInstallments { get; set; } = [];
        public List<UrgentActionResponse> UrgentActions { get; set; } = [];
    }
}
