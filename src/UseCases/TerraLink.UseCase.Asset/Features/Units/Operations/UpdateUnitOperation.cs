using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Units.Specifications;
using static TerraLink.Domain.Constants.Constant;
using TerraLink.UseCase.Asset.Features.Units.Endpoints;

namespace TerraLink.UseCase.Asset.Features.Units.Operations;

public sealed partial class UpdateUnitOperation(IRepository<UnitEntity> units)
    : IOperationHandler<UpdateUnitOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(UpdateUnitOperation.Request request, CancellationToken ct = default)
    {
        UpdateUnitFieldsSpec updateSpec = ObjectMapper.Map<UpdateUnitEndpoint.Request, UpdateUnitFieldsSpec>(request.Payload);

        int updated = await units.UpdateAsync(
            new UnitByIdSpec(request.Id),
            updateSpec,
            ct);

        return updated > 0
            ? new Success()
            : Errors.UpdateFaild;
    }
}
