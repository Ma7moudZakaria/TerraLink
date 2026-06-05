using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Sales.Features.Contracts.Operations
{
    public sealed partial class GetContractDetailsOperation
    {
        public sealed class Response
        {
            public Guid Id { get; set; }
            public required string ContractNumber { get; set; }
            public Guid ContractTypeId { get; set; }
            public required string ContractTypeName { get; set; }
            public Guid? PaymentMethodId { get; set; }
            public string? PaymentMethodName { get; set; }
            public DateTime ContractDate { get; set; }
            public decimal UnitPriceAtContract { get; set; }
            public decimal TotalPrice { get; set; }
            public bool IsInstallmentPlan { get; set; }
            public string? Notes { get; set; }
            public Guid? LandId { get; set; }
            public string? LandName { get; set; }
            public Guid? BuildingId { get; set; }
            public string? BuildingName { get; set; }
            public Guid? UnitId { get; set; }
            public string? UnitName { get; set; }
            public decimal? UnitArea { get; set; }
            public string? UnitType { get; set; }
            public decimal? UnitPrice { get; set; }
            public int? FloorNumber { get; set; }
            public Guid ClientId { get; set; }
            public required string ClientName { get; set; }
            public required string ClientEmail { get; set; }
            public required string ClientPhone { get; set; }
            public required string ClientNationalId { get; set; }
            public List<InstallmentDetailsResponse> Installments { get; set; } = [];
            public AttributesDictionary? Attachments { get; set; }
            public DateTime CreatedDate { get; set; }
            public Guid UserId { get; set; }
            public string? UserName { get; set; }
        }

        public sealed class InstallmentDetailsResponse
        {
            public Guid Id { get; set; }
            public required string Description { get; set; }
            public DateTime DueDate { get; set; }
            public decimal Amount { get; set; }
            public required string AmountText { get; set; }
        }
    }
}
