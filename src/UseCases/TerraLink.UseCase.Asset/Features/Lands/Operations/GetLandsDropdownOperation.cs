using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Lands.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Asset.Features.Lands.Operations;

public sealed partial class GetLandsDropdownOperation(IRepository<LandEntity> lands)
    : IOperationHandler<GetLandsDropdownOperation.Request, List<GetLandsDropdownOperation.Response>>
{
    public async Task<ErrorOr<List<GetLandsDropdownOperation.Response>>> HandleAsync(GetLandsDropdownOperation.Request request, CancellationToken ct = default)
    {
        IReadOnlyList<LandEntity> result = await lands.ListAsync(new ActiveLandsSpec(), ct);

        return result
            .Select(land => new GetLandsDropdownOperation.Response(land.Id, $"{land.Number} - {land.Name}"))
            .ToList();
    }
}
