using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Entities;
using TerraLink.UseCase.Lookup.Features.LookupItems.Specifications;
using TerraLink.UseCase.Lookup.Features.LookupSets.Specifications;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Operations;

public sealed partial class GetLookupItemByCodeOperation(
    IRepository<LookupSetEntity> lookupSets,
    IRepository<LookupItemEntity> lookupItems,
    IMapper mapper)
    : IOperationHandler<GetLookupItemByCodeOperation.Request, GetLookupItemByCodeOperation.Response>
{
    public async Task<ErrorOr<GetLookupItemByCodeOperation.Response>> HandleAsync(
        GetLookupItemByCodeOperation.Request request,
        CancellationToken ct = default)
    {
        LookupSetEntity? lookupSet = await lookupSets.GetAsync(new LookupSetByCodeSpec(request.SetCode), ct);

        if (lookupSet is null)
        {
            return Error.NotFound("LookupSet.NotFound", $"Lookup set with code '{request.SetCode}' was not found.");
        }

        LookupItemEntity? item = await lookupItems.GetAsync(new LookupItemDetailsSpec(lookupSet.Id, request.ItemCode), ct);

        if (item is null)
        {
            return Error.NotFound("LookupItem.NotFound", $"Lookup item with code '{request.ItemCode}' in set '{request.SetCode}' was not found.");
        }

        return mapper.Map<LookupItemEntity, GetLookupItemByCodeOperation.Response>(item);
    }
}
