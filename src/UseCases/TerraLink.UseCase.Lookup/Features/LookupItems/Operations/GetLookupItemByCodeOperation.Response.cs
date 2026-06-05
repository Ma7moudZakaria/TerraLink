using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Operations;

public sealed partial class GetLookupItemByCodeOperation
{
    public sealed record LookupSetResponse(Guid Id, string Code, Localized Descriptions);

    public sealed record Response(
        Guid Id,
        Guid LookupSetId,
        string Code,
        Localized Descriptions,
        AttributeDictionary? Metadata,
        bool IsActive,
        int SortOrder,
        DateTime CreatedDate,
        string? CreatedBy,
        DateTime? UpdatedDate,
        string? ModifiedBy,
        LookupSetResponse? LookupSet);
}
