using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Lands.Specifications;

namespace TerraLink.UseCase.Asset.Features.Lands.Operations;

public sealed partial class GetLandsOperation(IRepository<LandEntity> lands, IMapper mapper)
    : IOperationHandler<GetLandsOperation.Request, PagedList<GetLandsOperation.Response>>
{
    public async Task<ErrorOr<PagedList<GetLandsOperation.Response>>> HandleAsync(GetLandsOperation.Request request, CancellationToken ct = default)
    {
        PagedList<LandEntity> result = await lands.PagedAsync(
            new LandsListSpec(request.Payload),
            request.Payload.PageNumber,
            request.Payload.PageSize,
            ct);

        return new PagedList<GetLandsOperation.Response>
        {
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            Items = result.Items.Select(mapper.Map<LandEntity, GetLandsOperation.Response>).ToList()
        };
    }
}
