using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Entities;
using TerraLink.UseCase.Lookup.Features.LookupSets.Specifications;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Operations;

public sealed partial class GetLookupSetByCodeOperation(IRepository<LookupSetEntity> lookupSets, IMapper mapper)
    : IOperationHandler<GetLookupSetByCodeOperation.Request, GetLookupSetByCodeOperation.Response>
{
    public async Task<ErrorOr<GetLookupSetByCodeOperation.Response>> HandleAsync(
        GetLookupSetByCodeOperation.Request request,
        CancellationToken ct = default)
    {
        LookupSetEntity? lookupSet = await lookupSets.GetAsync(new LookupSetDetailsSpec(request.Code), ct);

        if (lookupSet is null)
        {
            return Error.NotFound("LookupSet.NotFound", $"Lookup set with code '{request.Code}' was not found.");
        }

        return mapper.Map<LookupSetEntity, GetLookupSetByCodeOperation.Response>(lookupSet);
    }
}
