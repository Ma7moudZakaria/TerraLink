using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.ValueObjects;
using TerraLink.Domain.Enums;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Operations;

public sealed partial class UpdateIncomingPaymentOperation
{
    public sealed record Request(
    Guid Id,
    Guid ContractId,
    Guid? ContractInstallmentId,
    Guid ClientId,
    IncomingPaymentSourceType SourceType,
    Guid TransactionTypeId,
    decimal Amount,
    Guid PaymentMethodId,
    DateTime PaymentDate,
    string Notes,
    AttributesDictionary? Attachments) : IOperationRequest<Success>;
}
