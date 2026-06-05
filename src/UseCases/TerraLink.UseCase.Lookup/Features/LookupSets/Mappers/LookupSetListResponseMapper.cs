using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Entities;
using TerraLink.UseCase.Lookup.Features.LookupSets.Operations;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Mappers;

public sealed class LookupSetListResponseMapper : IMapHandler<LookupSetEntity, GetAllLookupSetsOperation.Response>
{
    public GetAllLookupSetsOperation.Response Handler(LookupSetEntity source)
    {
        return new GetAllLookupSetsOperation.Response(
            source.Id,
            source.Code,
            source.Descriptions,
            source.IsActive,
            source.LookupItems.Count,
            source.CreatedDate);
    }
}
