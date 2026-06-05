using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Endpoints;

public sealed partial class CreateOutgoingPaymentEndpoint
{
    public sealed record Request(
        Guid ExpenseTypeId,
        Guid BeneficiaryId,
        Guid? UnitId,
        Guid? BuildingId,
        decimal Amount,
        Guid PaymentMethodId,
        DateTime PaymentDate,
        string Notes,
        AttributesDictionary? Attachments);
}
