using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Endpoints;

public sealed partial class GetOutgoingPaymentsEndpoint
{
    public sealed class Response
    {
        public Guid Id { get; set; }
        public required Localized Beneficiary { get; set; }
        public required string Code { get; set; }
        public required Localized ExpenseType { get; set; }
        public string? BuildingCode { get; set; }
        public string? UnitCode { get; set; }
        public required decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public required Localized PaymentMethod { get; set; }
    }
}
