namespace TerraLink.UseCase.Sales.Features.Clients.Endpoints
{
    public sealed partial class GetClientDetailsEndpoint
    {
        public sealed class Response
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Code { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public string NationalId { get; set; } = string.Empty;
            public List<GetClientOwnedUnitResponse> OwnedUnits { get; set; } = [];
        }

        public sealed class GetClientOwnedUnitResponse
        {
            public Guid ContractId { get; set; }
            public string ContractNumber { get; set; } = string.Empty;
            public DateTime ContractDate { get; set; }
            public Guid? UnitId { get; set; }
            public string? UnitNumber { get; set; }
            public string? UnitName { get; set; }
            public string? BuildingName { get; set; }
            public string? LandName { get; set; }
            public int? FloorNumber { get; set; }
            public decimal? UnitArea { get; set; }
            public int? NumberOfRooms { get; set; }
            public int? NumberOfBathrooms { get; set; }
            public decimal UnitPriceAtContract { get; set; }
            public decimal TotalPrice { get; set; }
            public bool IsInstallmentPlan { get; set; }
            public List<GetClientInstallmentDetailsResponse> Installments { get; set; } = [];
        }

        public sealed class GetClientInstallmentDetailsResponse
        {
            public Guid InstallmentId { get; set; }
            public string Description { get; set; } = string.Empty;
            public DateTime DueDate { get; set; }
            public decimal Amount { get; set; }
            public string AmountText { get; set; } = string.Empty;
            public bool IsPaid { get; set; }
            public decimal PaidAmount { get; set; }
            public decimal RemainingAmount { get; set; }
        }
    }
}
