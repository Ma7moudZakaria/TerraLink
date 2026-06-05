using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Entities;
using TerraLink.UseCase.Lookup.Features.LookupSets.Operations;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Mappers;

public sealed class LookupSetDetailsResponseMapper : IMapHandler<LookupSetEntity, GetLookupSetByCodeOperation.Response>
{
    public GetLookupSetByCodeOperation.Response Handler(LookupSetEntity source)
    {
        return new GetLookupSetByCodeOperation.Response(
            source.Id,
            source.Code,
            source.Descriptions,
            source.IsActive,
            source.CreatedDate,
            source.CreatedBy,
            source.UpdatedDate,
            source.ModifiedBy,
            source.LookupItems
                .OrderBy(item => item.SortOrder)
                .ThenBy(item => item.Code)
                .Select(item => new GetLookupSetByCodeOperation.ItemResponse(
                    item.Id,
                    item.Code,
                    item.Descriptions,
                    item.IsActive,
                    item.SortOrder))
                .ToList());
    }
}
