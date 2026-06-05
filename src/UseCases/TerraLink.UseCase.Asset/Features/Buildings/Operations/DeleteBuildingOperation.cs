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

public sealed partial class DeleteBuildingOperation(IRepository<BuildingEntity> buildings)
    : IOperationHandler<DeleteBuildingOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(DeleteBuildingOperation.Request request, CancellationToken ct = default)
    {
        int deleted = await buildings.UpdateAsync(
            new BuildingByIdSpec(request.Id),
            new SoftDeleteBuildingUpdateSpec(),
            ct);

        return deleted > 0
            ? new Success()
            : Errors.DeleteFaild;
    }
}
