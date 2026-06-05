namespace TerraLink.UseCase.Sales.Features.Dashboard.Operations
{
    public sealed partial class GetSalesDashboardOverviewOperation
    {
        public sealed class Response
        {
            public int TotalContracts { get; set; }
            public decimal TotalSalesValue { get; set; }
            public decimal TotalIncomingPayments { get; set; }
            public decimal InstallmentsDueThisMonth { get; set; }
            public decimal InstallmentsOverdue { get; set; }
            public decimal CollectionRatePercentage { get; set; }
            public int NewClientsThisMonth { get; set; }
            public List<UrgentInstallmentActionResponse> UrgentActions { get; set; } = [];
            public List<DashboardInstallmentStatusSummaryResponse> InstallmentStatusSummary { get; set; } = [];
        }

        public sealed class DashboardInstallmentStatusSummaryResponse
        {
            public string Status { get; set; } = string.Empty;
            public int Count { get; set; }
            public decimal Amount { get; set; }
        }

        public sealed class UrgentInstallmentActionResponse
        {
            public Guid InstallmentId { get; set; }
            public Guid ContractId { get; set; }
            public string ContractNumber { get; set; } = string.Empty;
            public Guid ClientId { get; set; }
            public string ClientName { get; set; } = string.Empty;
            public DateTime DueDate { get; set; }
            public decimal InstallmentAmount { get; set; }
            public decimal PaidAmount { get; set; }
            public decimal RemainingAmount { get; set; }
            public int DaysToDue { get; set; }
            public string Status { get; set; } = string.Empty;
        }
    }
}
