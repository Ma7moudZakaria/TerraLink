using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Units.Specifications;

namespace TerraLink.UseCase.Asset.Features.Units.Operations;

public sealed partial class GetUnitDetailsOperation(IRepository<UnitEntity> units, IMapper mapper)
    : IOperationHandler<GetUnitDetailsOperation.Request, GetUnitDetailsOperation.Response>
{
    public async Task<ErrorOr<GetUnitDetailsOperation.Response>> HandleAsync(GetUnitDetailsOperation.Request request, CancellationToken ct = default)
    {
        UnitEntity? unit = await units.GetAsync(new UnitDetailsSpec(request.Id), ct);

        if (unit is null)
        {
            return Error.NotFound("Unit.NotFound", "Unit was not found.");
        }

        return mapper.Map<UnitEntity, GetUnitDetailsOperation.Response>(unit);
    }
}
