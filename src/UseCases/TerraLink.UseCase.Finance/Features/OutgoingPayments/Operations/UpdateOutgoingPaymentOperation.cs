using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.OutgoingPayments.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Operations;

public sealed partial class UpdateOutgoingPaymentOperation(IRepository<OutgoingPaymentEntity> outgoingPayments)
    : IOperationHandler<UpdateOutgoingPaymentOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(UpdateOutgoingPaymentOperation.Request request, CancellationToken ct = default)
    {
        if (request.Amount <= 0)
            return Error.Validation(ErrorCode.NoItemCorrect, "Amount must be greater than zero.");

        var update = new UpdateOutgoingPaymentFieldsSpec
        {
            ExpenseTypeId = request.ExpenseTypeId,
            BeneficiaryId = request.BeneficiaryId,
            UnitId = request.UnitId,
            BuildingId = request.BuildingId,
            Amount = request.Amount,
            PaymentMethodId = request.PaymentMethodId,
            PaymentDate = request.PaymentDate,
            Notes = request.Notes,
            Attachments = request.Attachments
        };

        if (await outgoingPayments.UpdateAsync(new OutgoingPaymentByIdSpec(request.Id), update, ct) > 0)
            return new Success();

        return Errors.UpdateFaild;
    }
}
