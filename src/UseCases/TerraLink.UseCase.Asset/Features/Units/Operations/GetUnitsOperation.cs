using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Units.Specifications;

namespace TerraLink.UseCase.Asset.Features.Units.Operations;

public sealed partial class GetUnitsOperation(IRepository<UnitEntity> units, IMapper mapper)
    : IOperationHandler<GetUnitsOperation.Request, PagedList<GetUnitsOperation.Response>>
{
    public async Task<ErrorOr<PagedList<GetUnitsOperation.Response>>> HandleAsync(GetUnitsOperation.Request request, CancellationToken ct = default)
    {
        PagedList<UnitEntity> result = await units.PagedAsync(
            new UnitsListSpec(request.Payload),
            request.Payload.PageNumber,
            request.Payload.PageSize,
            ct);

        return new PagedList<GetUnitsOperation.Response>
        {
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            Items = result.Items.Select(mapper.Map<UnitEntity, GetUnitsOperation.Response>).ToList()
        };
    }
}
