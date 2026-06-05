using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Operations;

public sealed partial class GetLookupItemsBySetIdOperation
{
    public sealed record Response(Guid Id, Guid LookupSetId, string Code, Localized Descriptions, bool IsActive, int SortOrder);
}
