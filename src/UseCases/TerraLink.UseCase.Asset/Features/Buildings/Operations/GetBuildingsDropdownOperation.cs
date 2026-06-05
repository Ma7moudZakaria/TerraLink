using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Interfaces.Services;
using TerraLink.UseCase.Asset.Features.Buildings.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Asset.Features.Buildings.Operations;

public sealed partial class GetBuildingsDropdownOperation(IRepository<BuildingEntity> buildings)
    : IOperationHandler<GetBuildingsDropdownOperation.Request, List<GetBuildingsDropdownOperation.Response>>
{
    public async Task<ErrorOr<List<GetBuildingsDropdownOperation.Response>>> HandleAsync(GetBuildingsDropdownOperation.Request request, CancellationToken ct = default)
    {
        IReadOnlyList<BuildingEntity> result = await buildings.ListAsync(new ActiveBuildingsSpec(request.LandId), ct);

        return result
            .Select(building => new GetBuildingsDropdownOperation.Response(building.Id, $"{building.Number} - {building.Name}"))
            .ToList();
    }
}
