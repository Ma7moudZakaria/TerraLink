using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Buildings.Specifications;

namespace TerraLink.UseCase.Asset.Features.Buildings.Operations;

public sealed partial class GetBuildingDetailsOperation(IRepository<BuildingEntity> buildings, IMapper mapper)
    : IOperationHandler<GetBuildingDetailsOperation.Request, GetBuildingDetailsOperation.Response>
{
    public async Task<ErrorOr<GetBuildingDetailsOperation.Response>> HandleAsync(GetBuildingDetailsOperation.Request request, CancellationToken ct = default)
    {
        BuildingEntity? building = await buildings.GetAsync(new BuildingDetailsSpec(request.Id), ct);

        if (building is null)
        {
            return Error.NotFound("Building.NotFound", "Building was not found.");
        }

        return mapper.Map<BuildingEntity, GetBuildingDetailsOperation.Response>(building);
    }
}
