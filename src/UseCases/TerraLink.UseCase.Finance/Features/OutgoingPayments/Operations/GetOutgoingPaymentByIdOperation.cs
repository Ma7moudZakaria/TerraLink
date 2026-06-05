using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.OutgoingPayments.Specifications;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Operations;

public sealed partial class GetOutgoingPaymentByIdOperation(IRepository<OutgoingPaymentEntity> outgoingPayments, IMapper mapper)
    : IOperationHandler<GetOutgoingPaymentByIdOperation.Request, GetOutgoingPaymentByIdOperation.Response>
{
    public async Task<ErrorOr<GetOutgoingPaymentByIdOperation.Response>> HandleAsync(
        GetOutgoingPaymentByIdOperation.Request request,
        CancellationToken ct = default)
    {
        OutgoingPaymentEntity? payment = await outgoingPayments.GetAsync(new OutgoingPaymentDetailsSpec(request.Id), ct);

        if (payment is null)
        {
            return Error.NotFound("OutgoingPayment.NotFound", $"Outgoing payment with id '{request.Id}' was not found.");
        }

        return mapper.Map<OutgoingPaymentEntity, GetOutgoingPaymentByIdOperation.Response>(payment);
    }
}
