using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.OutgoingPayments.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Operations;

public sealed partial class DeleteOutgoingPaymentOperation(IRepository<OutgoingPaymentEntity> outgoingPayments)
    : IOperationHandler<DeleteOutgoingPaymentOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(DeleteOutgoingPaymentOperation.Request request, CancellationToken ct = default)
    {
        if (await outgoingPayments.UpdateAsync(new OutgoingPaymentByIdSpec(request.Id), new SoftDeleteOutgoingPaymentUpdateSpec(), ct) > 0)
            return new Success();

        return Errors.DeleteFaild;
    }
}
