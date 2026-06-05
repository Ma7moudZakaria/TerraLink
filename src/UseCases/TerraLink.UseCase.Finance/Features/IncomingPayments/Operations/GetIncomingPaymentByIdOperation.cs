using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.IncomingPayments.Specifications;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Operations;

public sealed partial class GetIncomingPaymentByIdOperation(IRepository<IncomingPaymentEntity> incomingPayments, IMapper mapper)
    : IOperationHandler<GetIncomingPaymentByIdOperation.Request, GetIncomingPaymentByIdOperation.Response>
{
    public async Task<ErrorOr<GetIncomingPaymentByIdOperation.Response>> HandleAsync(
        GetIncomingPaymentByIdOperation.Request request,
        CancellationToken ct = default)
    {
        IncomingPaymentEntity? payment = await incomingPayments.GetAsync(new IncomingPaymentDetailsSpec(request.Id), ct);

        if (payment is null)
        {
            return Error.NotFound("IncomingPayment.NotFound", $"Incoming payment with id '{request.Id}' was not found.");
        }

        return mapper.Map<IncomingPaymentEntity, GetIncomingPaymentByIdOperation.Response>(payment);
    }
}
