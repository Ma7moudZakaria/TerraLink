using LowCodeHub.QueryableExtensions.Models;

namespace TerraLink.UseCase.Sales.Features.Contracts.Endpoints
{
    public sealed partial class GetAllContractsEndpoint
    {
        public sealed class Request : PagedQueryBase
        {
            public string? ContractNumber { get; set; }
            public string? ClientName { get; set; }
            public string? UnitNumber { get; set; }
            public decimal? TotalPrice { get; set; }
            public decimal? UnitPriceAtContract { get; set; }
            public Guid? ContractTypeId { get; set; }
            public DateTime? ContractDate { get; set; }
            public bool? IsInstallmentPlan { get; set; }
        }
    }
}
