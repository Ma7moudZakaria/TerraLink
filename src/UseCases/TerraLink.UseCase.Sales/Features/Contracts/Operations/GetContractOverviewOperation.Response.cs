namespace TerraLink.UseCase.Sales.Features.Contracts.Operations
{
    public sealed partial class GetContractOverviewOperation
    {
        public sealed class Response
        {
            public int TotalContracts { get; set; }
            public int ContractsThisMonth { get; set; }
            public decimal MonthlySalesValue { get; set; }
            public decimal AnnualSalesValue { get; set; }
            public decimal GrowthPercentage { get; set; }
            public decimal DueInstallmentsThisMonth { get; set; }
            public decimal OverdueInstallmentsAmount { get; set; }
            public List<MonthlySalesPointResponse> MonthlySales { get; set; } = [];
            public List<ContractUrgentActionResponse> UrgentActions { get; set; } = [];
        }

        public sealed class MonthlySalesPointResponse
        {
            public int Month { get; set; }
            public decimal Value { get; set; }
        }

        public sealed class ContractUrgentActionResponse
        {
            public Guid ContractId { get; set; }
            public string ContractNumber { get; set; } = string.Empty;
            public string ClientName { get; set; } = string.Empty;
            public DateTime DueDate { get; set; }
            public decimal RemainingAmount { get; set; }
            public string Status { get; set; } = string.Empty;
        }
    }
}
