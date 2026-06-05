using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Asset.Features.Units.Operations
{
    public sealed partial class GetUnitDetailsOperation
    {
        public sealed class Response
        {
            public required Guid Id { get; set; }
            public string? Description { get; set; }
            public required string Building { get; set; }
            public required string Land { get; set; }
            public required decimal Area { get; set; }
            public required int FloorNumber { get; set; }
            public required int NumberOfRooms { get; set; }
            public required int NumberOfBatEmployeeooms { get; set; }
            public required Localized UnitType { get; set; }
            public required Localized UnitStatus { get; set; }
            public required string Name { get; set; }
            public required decimal Price { get; set; }
            public required string Number { get; set; }
            public bool? HasBalcony { get; set; }
            public bool? HasGarage { get; set; }
            public bool? HasCentralAC { get; set; }
            public required Localized FinishingType { get; set; }
            public Guid BuildingId { get; set; }
            public Guid LandId { get; set; }
            public Guid UnitTypeId { get; set; }
            public Guid UnitStatusId { get; set; }
            public Guid FinishingTypeId { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public bool IsPurchased { get; set; }
            public UnitPurchaseDetailsResponse? PurchaseDetails { get; set; }
            public AttributesDictionary? Attachments { get; set; }
        }

        public sealed class UnitPurchaseDetailsResponse
        {
            public Guid ContractId { get; set; }
            public string ContractNumber { get; set; } = string.Empty;
            public DateTime ContractDate { get; set; }
            public decimal TotalPrice { get; set; }
            public decimal UnitPriceAtContract { get; set; }
            public bool IsInstallmentPlan { get; set; }
            public string? Notes { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public UnitPurchasedClientResponse? Client { get; set; }
            public List<UnitInstallmentDetailsResponse> Installments { get; set; } = [];
        }

        public sealed class UnitPurchasedClientResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string? Phone { get; set; }
            public string? Email { get; set; }
            public string? NationalId { get; set; }
            public string? Address { get; set; }
        }

        public sealed class UnitInstallmentDetailsResponse
        {
            public Guid Id { get; set; }
            public string Description { get; set; } = string.Empty;
            public DateTime DueDate { get; set; }
            public decimal Amount { get; set; }
            public string? AmountText { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? UpdatedDate { get; set; }
        }
    }
}
