using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Endpoints;

public sealed partial class GetLookupItemsBySetIdEndpoint
{
    public sealed record Response(Guid Id, Guid LookupSetId, string Code, Localized Descriptions, bool IsActive, int SortOrder);
}
