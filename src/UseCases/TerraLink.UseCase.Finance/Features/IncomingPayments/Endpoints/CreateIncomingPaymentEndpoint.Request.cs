using LowCodeHub.QueryableExtensions.ValueObjects;
using TerraLink.Domain.Enums;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Endpoints;

public sealed partial class CreateIncomingPaymentEndpoint
{
    public sealed record Request(
        Guid ContractId,
        Guid? ContractInstallmentId,
        Guid ClientId,
        IncomingPaymentSourceType SourceType,
        Guid TransactionTypeId,
        decimal Amount,
        Guid PaymentMethodId,
        DateTime PaymentDate,
        string Notes,
        AttributesDictionary? Attachments);
}
