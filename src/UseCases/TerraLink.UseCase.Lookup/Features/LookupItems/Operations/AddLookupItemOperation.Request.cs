using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Operations;

public sealed partial class AddLookupItemOperation
{
    public sealed record Request(
    Guid LookupSetId,
    string Code,
    Localized? Descriptions,
    int SortOrder,
    AttributeDictionary? Metadata) : IOperationRequest<Response>;
}
