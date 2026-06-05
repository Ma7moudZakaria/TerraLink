using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Units.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Asset.Features.Units.Operations;

public sealed partial class DeleteUnitOperation(IRepository<UnitEntity> units)
    : IOperationHandler<DeleteUnitOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(DeleteUnitOperation.Request request, CancellationToken ct = default)
    {
        int deleted = await units.UpdateAsync(
            new UnitByIdSpec(request.Id),
            new SoftDeleteUnitUpdateSpec(),
            ct);

        return deleted > 0
            ? new Success()
            : Errors.DeleteFaild;
    }
}
