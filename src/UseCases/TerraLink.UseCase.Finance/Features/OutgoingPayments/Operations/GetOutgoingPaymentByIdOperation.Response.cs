using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Operations;

public sealed partial class GetOutgoingPaymentByIdOperation
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
        public string Notes { get; set; } = string.Empty;
        public AttributesDictionary? Attachments { get; set; }
    }
}
