using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Sales.Features.Contracts.Endpoints
{
    public sealed partial class CreateContractEndpoint
    {
        public sealed class Request
        {
            public DateTime ContractDate { get; set; }
            public decimal TotalPrice { get; set; }
            public Guid ContractTypeId { get; set; }
            public required string ClientNationalId { get; set; }
            public string? Notes { get; set; }
            public Guid? LandId { get; set; }
            public Guid? BuildingId { get; set; }
            public Guid? UnitId { get; set; }
            public decimal UnitPriceAtContract { get; set; }
            public bool IsInstallmentPlan { get; set; }
            public List<InstallmentItemRequest>? Installments { get; set; }
            public AttributesDictionary? Attachments { get; set; }
        }

        public sealed class InstallmentItemRequest
        {
            public Guid? Id { get; set; }
            public required string Description { get; set; }
            public DateTime DueDate { get; set; }
            public decimal Amount { get; set; }
            public required string AmountText { get; set; }
        }
    }
}
