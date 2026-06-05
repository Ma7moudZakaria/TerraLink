namespace TerraLink.UseCase.Sales.Features.Contracts.Endpoints
{
    public sealed partial class GetAllContractsEndpoint
    {
        public sealed class Response
        {
            public Guid Id { get; set; }
            public required string ContractNumber { get; set; }
            public string? ClientName { get; set; }
            public string? LandName { get; set; }
            public string? BuildingName { get; set; }
            public string? UnitName { get; set; }
            public string? ContractType { get; set; }
            public DateTime? ContractDate { get; set; }
            public decimal TotalPrice { get; set; }
            public decimal UnitPriceAtContract { get; set; }
            public bool IsInstallmentPlan { get; set; }
            public string? Notes { get; set; }
            public DateTime CreatedDate { get; set; }
        }
    }
}
