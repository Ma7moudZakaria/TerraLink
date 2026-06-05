using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.IncomingPayments.Specifications;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Operations;

public sealed partial class GetIncomingPaymentsOperation(IRepository<IncomingPaymentEntity> incomingPayments, IMapper mapper)
    : IOperationHandler<GetIncomingPaymentsOperation.Request, PagedList<GetIncomingPaymentsOperation.Response>>
{
    public async Task<ErrorOr<PagedList<GetIncomingPaymentsOperation.Response>>> HandleAsync(
        GetIncomingPaymentsOperation.Request request,
        CancellationToken ct = default)
    {
        PagedList<IncomingPaymentEntity> result = await incomingPayments.PagedAsync(
            new IncomingPaymentsListSpec(
                request.Code,
                request.ContractCode,
                request.ClientName,
                request.UnitCode,
                request.Amount,
                request.PaymentDate),
            request.Page,
            request.PageSize,
            ct);

        return new PagedList<GetIncomingPaymentsOperation.Response>
        {
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            Items = result.Items.Select(mapper.Map<IncomingPaymentEntity, GetIncomingPaymentsOperation.Response>).ToList()
        };
    }
}
