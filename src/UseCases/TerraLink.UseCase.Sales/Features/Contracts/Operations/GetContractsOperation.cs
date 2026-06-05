using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Contracts.Specifications;

namespace TerraLink.UseCase.Sales.Features.Contracts.Operations;

public sealed partial class GetContractsOperation(IRepository<ContractEntity> contracts, IMapper mapper)
    : IOperationHandler<GetContractsOperation.Request, PagedList<GetContractsOperation.Response>>
{
    public async Task<ErrorOr<PagedList<GetContractsOperation.Response>>> HandleAsync(GetContractsOperation.Request request, CancellationToken ct = default)
    {
        PagedList<ContractEntity> result = await contracts.PagedAsync(
            new ContractsListSpec(request.Payload),
            request.Payload.PageNumber,
            request.Payload.PageSize,
            ct);

        return new PagedList<GetContractsOperation.Response>
        {
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            Items = result.Items.Select(mapper.Map<ContractEntity, GetContractsOperation.Response>).ToList()
        };
    }
}
