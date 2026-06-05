using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Entities;
using TerraLink.UseCase.Lookup.Features.LookupItems.Specifications;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Operations;

public sealed partial class AddLookupItemOperation(IRepository<LookupItemEntity> lookupItems)
    : IOperationHandler<AddLookupItemOperation.Request, AddLookupItemOperation.Response>
{
    public async Task<ErrorOr<AddLookupItemOperation.Response>> HandleAsync(Request request, CancellationToken ct = default)
    {
        if (await lookupItems.CountAsync(new LookupItemBySetIdAndCodeSpec(request.LookupSetId, request.Code), ct) > 0)
        {
            return Error.Conflict("LookupItem.DuplicateCode", $"Lookup item with code '{request.Code}' already exists in this set.");
        }

        Guid id = Guid.CreateVersion7();
        lookupItems.Add(new CreateLookupItemAddSpec(
            id,
            request.LookupSetId,
            request.Code,
            request.Descriptions,
            request.SortOrder,
            request.Metadata));

        await lookupItems.SaveChangesAsync(ct);
        return new AddLookupItemOperation.Response { Id = id };
    }
}
