using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Lands.Specifications;
using static TerraLink.Domain.Constants.Constant;
using TerraLink.UseCase.Asset.Features.Lands.Endpoints;

namespace TerraLink.UseCase.Asset.Features.Lands.Operations;

public sealed partial class UpdateLandOperation(IRepository<LandEntity> lands)
    : IOperationHandler<UpdateLandOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(UpdateLandOperation.Request request, CancellationToken ct = default)
    {
        UpdateLandFieldsSpec updateSpec = ObjectMapper.Map<UpdateLandEndpoint.Request, UpdateLandFieldsSpec>(request.Payload);

        int updated = await lands.UpdateAsync(
            new LandByIdSpec(request.Id),
            updateSpec,
            ct);

        return updated > 0
            ? new Success()
            : Errors.UpdateFaild;
    }
}
