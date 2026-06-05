using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Entities;
using TerraLink.UseCase.Lookup.Features.LookupSets.Specifications;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Operations;

public sealed partial class AddLookupSetOperation(IRepository<LookupSetEntity> lookupSets)
    : IOperationHandler<AddLookupSetOperation.Request, AddLookupSetOperation.Response>
{
    public async Task<ErrorOr<AddLookupSetOperation.Response>> HandleAsync(AddLookupSetOperation.Request request, CancellationToken ct = default)
    {
        if (await lookupSets.CountAsync(new LookupSetByCodeSpec(request.Code), ct) > 0)
        {
            return Error.Conflict("LookupSet.DuplicateCode", $"Lookup set with code '{request.Code}' already exists.");
        }

        Guid id = Guid.CreateVersion7();
        lookupSets.Add(new CreateLookupSetAddSpec(id, request.Code, request.Descriptions));

        await lookupSets.SaveChangesAsync(ct);
        return new AddLookupSetOperation.Response { Id = id };
    }
}
