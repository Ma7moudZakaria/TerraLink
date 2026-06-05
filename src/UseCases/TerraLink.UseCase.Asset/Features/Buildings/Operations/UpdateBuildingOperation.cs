using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Interfaces.Services;
using TerraLink.UseCase.Asset.Features.Buildings.Specifications;
using static TerraLink.Domain.Constants.Constant;
using TerraLink.UseCase.Asset.Features.Buildings.Endpoints;

namespace TerraLink.UseCase.Asset.Features.Buildings.Operations;

public sealed partial class UpdateBuildingOperation(IRepository<BuildingEntity> buildings)
    : IOperationHandler<UpdateBuildingOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(UpdateBuildingOperation.Request request, CancellationToken ct = default)
    {
        UpdateBuildingFieldsSpec updateSpec = ObjectMapper.Map<UpdateBuildingEndpoint.Request, UpdateBuildingFieldsSpec>(request.Payload);

        int updated = await buildings.UpdateAsync(
            new BuildingByIdSpec(request.Id),
            updateSpec,
            ct);

        return updated > 0
            ? new Success()
            : Errors.UpdateFaild;
    }
}
