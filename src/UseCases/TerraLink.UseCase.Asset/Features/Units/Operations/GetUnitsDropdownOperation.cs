using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Units.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Asset.Features.Units.Operations;

public sealed partial class GetUnitsDropdownOperation(IRepository<UnitEntity> units)
    : IOperationHandler<GetUnitsDropdownOperation.Request, List<GetUnitsDropdownOperation.Response>>
{
    public async Task<ErrorOr<List<GetUnitsDropdownOperation.Response>>> HandleAsync(GetUnitsDropdownOperation.Request request, CancellationToken ct = default)
    {
        IReadOnlyList<UnitEntity> result = await units.ListAsync(new ActiveUnitsSpec(request.BuildingId, request.LandId), ct);

        return result
            .Select(unit => new GetUnitsDropdownOperation.Response(unit.Id, $"{unit.Number} - {unit.Name}"))
            .ToList();
    }
}
