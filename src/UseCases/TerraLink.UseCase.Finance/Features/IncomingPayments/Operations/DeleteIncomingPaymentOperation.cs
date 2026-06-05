using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.IncomingPayments.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Operations;

public sealed partial class DeleteIncomingPaymentOperation(IRepository<IncomingPaymentEntity> incomingPayments)
    : IOperationHandler<DeleteIncomingPaymentOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(DeleteIncomingPaymentOperation.Request request, CancellationToken ct = default)
    {
        if (await incomingPayments.UpdateAsync(new IncomingPaymentByIdSpec(request.Id), new SoftDeleteIncomingPaymentUpdateSpec(), ct) > 0)
            return new Success();

        return Errors.DeleteFaild;
    }
}
