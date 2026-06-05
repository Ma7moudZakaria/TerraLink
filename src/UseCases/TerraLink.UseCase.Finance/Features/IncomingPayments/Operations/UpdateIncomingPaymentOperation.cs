using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.IncomingPayments.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Operations;

public sealed partial class UpdateIncomingPaymentOperation(
    IRepository<IncomingPaymentEntity> incomingPayments,
    IRepository<ContractEntity> contracts)
    : IOperationHandler<UpdateIncomingPaymentOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(UpdateIncomingPaymentOperation.Request request, CancellationToken ct = default)
    {
        Error? validationError = await IncomingPaymentValidation.ValidateAsync(
            contracts,
            incomingPayments,
            request.ContractId,
            request.ClientId,
            request.SourceType,
            request.ContractInstallmentId,
            request.Amount,
            request.Id,
            ct);

        if (validationError is not null)
            return validationError.Value;

        var update = new UpdateIncomingPaymentFieldsSpec
        {
            ContractId = request.ContractId,
            ClientId = request.ClientId,
            ContractInstallmentId = request.ContractInstallmentId,
            SourceType = request.SourceType,
            TransactionTypeId = request.TransactionTypeId,
            Amount = request.Amount,
            PaymentMethodId = request.PaymentMethodId,
            PaymentDate = request.PaymentDate,
            Notes = request.Notes,
            Attachments = request.Attachments
        };

        if (await incomingPayments.UpdateAsync(new IncomingPaymentByIdSpec(request.Id), update, ct) > 0)
            return new Success();

        return Errors.UpdateFaild;
    }
}
