using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Lands.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Asset.Features.Lands.Operations;

public sealed partial class DeleteLandOperation(IRepository<LandEntity> lands)
    : IOperationHandler<DeleteLandOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(DeleteLandOperation.Request request, CancellationToken ct = default)
    {
        int deleted = await lands.UpdateAsync(
            new LandByIdSpec(request.Id),
            new SoftDeleteLandUpdateSpec(),
            ct);

        return deleted > 0
            ? new Success()
            : Errors.DeleteFaild;
    }
}
