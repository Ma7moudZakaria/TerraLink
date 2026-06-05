using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Interfaces.Services;
using TerraLink.UseCase.Finance.Features.IncomingPayments.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Operations;

public sealed partial class CreateIncomingPaymentOperation(
    IRepository<IncomingPaymentEntity> incomingPayments,
    IRepository<ContractEntity> contracts,
    ICodeGeneratorService codeGenerator)
    : IOperationHandler<CreateIncomingPaymentOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(CreateIncomingPaymentOperation.Request request, CancellationToken ct = default)
    {
        Error? validationError = await IncomingPaymentValidation.ValidateAsync(
            contracts,
            incomingPayments,
            request.ContractId,
            request.ClientId,
            request.SourceType,
            request.ContractInstallmentId,
            request.Amount,
            null,
            ct);

        if (validationError is not null)
            return validationError.Value;

        string code = await codeGenerator.GenerateCodeAsync<IncomingPaymentEntity>(ct);

        incomingPayments.Add(new CreateIncomingPaymentAddSpec(
            Guid.CreateVersion7(),
            code,
            request.ContractId,
            request.ContractInstallmentId,
            request.ClientId,
            request.SourceType,
            request.TransactionTypeId,
            request.Amount,
            request.PaymentMethodId,
            request.PaymentDate,
            request.Notes,
            request.Attachments));

        if (await incomingPayments.SaveChangesAsync(ct) > 0)
            return new Success();

        return Errors.CreateFaild;
    }
}
