using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Entities;
using TerraLink.UseCase.Lookup.Features.LookupItems.Operations;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Mappers;

public sealed class LookupItemDetailsResponseMapper : IMapHandler<LookupItemEntity, GetLookupItemByCodeOperation.Response>
{
    public GetLookupItemByCodeOperation.Response Handler(LookupItemEntity source)
    {
        return new GetLookupItemByCodeOperation.Response(
            source.Id,
            source.LookupSetId,
            source.Code,
            source.Descriptions,
            source.Metadata,
            source.IsActive,
            source.SortOrder,
            source.CreatedDate,
            source.CreatedBy,
            source.UpdatedDate,
            source.ModifiedBy,
            source.LookupSet is null
                ? null
                : new GetLookupItemByCodeOperation.LookupSetResponse(
                    source.LookupSet.Id,
                    source.LookupSet.Code,
                    source.LookupSet.Descriptions));
    }
}
