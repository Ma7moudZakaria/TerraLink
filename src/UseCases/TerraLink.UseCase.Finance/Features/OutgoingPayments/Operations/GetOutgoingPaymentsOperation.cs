using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.OutgoingPayments.Specifications;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Operations;

public sealed partial class GetOutgoingPaymentsOperation(IRepository<OutgoingPaymentEntity> outgoingPayments, IMapper mapper)
    : IOperationHandler<GetOutgoingPaymentsOperation.Request, PagedList<GetOutgoingPaymentsOperation.Response>>
{
    public async Task<ErrorOr<PagedList<GetOutgoingPaymentsOperation.Response>>> HandleAsync(
        GetOutgoingPaymentsOperation.Request request,
        CancellationToken ct = default)
    {
        PagedList<OutgoingPaymentEntity> result = await outgoingPayments.PagedAsync(
            new OutgoingPaymentsListSpec(
                request.Code,
                request.UnitCode,
                request.BuildingCode,
                request.ExpenseTypeId,
                request.BeneficiaryId,
                request.IsUnitRelated,
                request.Amount,
                request.PaymentDate),
            request.Page,
            request.PageSize,
            ct);

        return new PagedList<GetOutgoingPaymentsOperation.Response>
        {
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            Items = result.Items.Select(mapper.Map<OutgoingPaymentEntity, GetOutgoingPaymentsOperation.Response>).ToList()
        };
    }
}
