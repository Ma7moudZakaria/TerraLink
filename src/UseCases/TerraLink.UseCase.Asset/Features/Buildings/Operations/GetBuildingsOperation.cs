using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Buildings.Specifications;

namespace TerraLink.UseCase.Asset.Features.Buildings.Operations;

public sealed partial class GetBuildingsOperation(IRepository<BuildingEntity> buildings, IMapper mapper)
    : IOperationHandler<GetBuildingsOperation.Request, PagedList<GetBuildingsOperation.Response>>
{
    public async Task<ErrorOr<PagedList<GetBuildingsOperation.Response>>> HandleAsync(GetBuildingsOperation.Request request, CancellationToken ct = default)
    {
        PagedList<BuildingEntity> result = await buildings.PagedAsync(
            new BuildingsListSpec(request.Payload),
            request.Payload.PageNumber,
            request.Payload.PageSize,
            ct);

        return new PagedList<GetBuildingsOperation.Response>
        {
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            Items = result.Items.Select(mapper.Map<BuildingEntity, GetBuildingsOperation.Response>).ToList()
        };
    }
}
