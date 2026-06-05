using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Endpoints;

public sealed partial class AddLookupItemEndpoint
{
    public sealed record Request(
        Guid LookupSetId,
        string Code,
        Localized? Descriptions,
        int SortOrder,
        AttributeDictionary? Metadata);
}
