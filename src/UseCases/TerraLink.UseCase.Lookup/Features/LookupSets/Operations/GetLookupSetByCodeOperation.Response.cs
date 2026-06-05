using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Operations;

public sealed partial class GetLookupSetByCodeOperation
{
    public sealed record ItemResponse(Guid Id, string Code, Localized Descriptions, bool IsActive, int SortOrder);

    public sealed record Response(
        Guid Id,
        string Code,
        Localized Descriptions,
        bool IsActive,
        DateTime CreatedDate,
        string? CreatedBy,
        DateTime? UpdatedDate,
        string? ModifiedBy,
        List<ItemResponse> Items);
}
