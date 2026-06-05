using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Operations;

public sealed partial class CreateOutgoingPaymentOperation
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
    AttributesDictionary? Attachments) : IOperationRequest<Success>;
}
