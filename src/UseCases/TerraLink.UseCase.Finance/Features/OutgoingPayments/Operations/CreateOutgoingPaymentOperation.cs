using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Interfaces.Services;
using TerraLink.UseCase.Finance.Features.OutgoingPayments.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Operations;

public sealed partial class CreateOutgoingPaymentOperation(IRepository<OutgoingPaymentEntity> outgoingPayments, ICodeGeneratorService codeGenerator)
    : IOperationHandler<CreateOutgoingPaymentOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(CreateOutgoingPaymentOperation.Request request, CancellationToken ct = default)
    {
        if (request.Amount <= 0)
            return Error.Validation(ErrorCode.NoItemCorrect, "Amount must be greater than zero.");

        string code = await codeGenerator.GenerateCodeAsync<OutgoingPaymentEntity>(ct);

        outgoingPayments.Add(new CreateOutgoingPaymentAddSpec(
            Guid.CreateVersion7(),
            code,
            request.ExpenseTypeId,
            request.BeneficiaryId,
            request.UnitId,
            request.BuildingId,
            request.Amount,
            request.PaymentMethodId,
            request.PaymentDate,
            request.Notes,
            request.Attachments));

        if (await outgoingPayments.SaveChangesAsync(ct) > 0)
            return new Success();

        return Errors.CreateFaild;
    }
}
