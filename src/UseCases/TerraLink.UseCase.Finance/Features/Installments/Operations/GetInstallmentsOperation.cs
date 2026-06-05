using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.Installments.Specifications;

namespace TerraLink.UseCase.Finance.Features.Installments.Operations;

public sealed partial class GetInstallmentsOperation(IRepository<ContractInstallmentEntity> installments, IMapper mapper)
    : IOperationHandler<GetInstallmentsOperation.Request, PagedList<GetInstallmentsOperation.Response>>
{
    public async Task<ErrorOr<PagedList<GetInstallmentsOperation.Response>>> HandleAsync(GetInstallmentsOperation.Request request, CancellationToken ct = default)
    {
        PagedList<ContractInstallmentEntity> result = await installments.PagedAsync(
            new InstallmentsListSpec(request.ContractId, request.ClientName, request.Unit),
            request.Page,
            request.PageSize,
            ct);

        return new PagedList<GetInstallmentsOperation.Response>
        {
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            Items = result.Items.Select(mapper.Map<ContractInstallmentEntity, GetInstallmentsOperation.Response>).ToList()
        };
    }
}
