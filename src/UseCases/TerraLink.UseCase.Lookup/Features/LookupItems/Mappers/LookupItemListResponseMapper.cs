using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Entities;
using TerraLink.UseCase.Lookup.Features.LookupItems.Operations;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Mappers;

public sealed class LookupItemListResponseMapper : IMapHandler<LookupItemEntity, GetLookupItemsBySetIdOperation.Response>
{
    public GetLookupItemsBySetIdOperation.Response Handler(LookupItemEntity source)
    {
        return new GetLookupItemsBySetIdOperation.Response(
            source.Id,
            source.LookupSetId,
            source.Code,
            source.Descriptions,
            source.IsActive,
            source.SortOrder);
    }
}
