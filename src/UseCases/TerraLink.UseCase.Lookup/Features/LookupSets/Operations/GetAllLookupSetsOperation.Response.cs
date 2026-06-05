using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Operations;

public sealed partial class GetAllLookupSetsOperation
{
    public sealed record Response(Guid Id, string Code, Localized Descriptions, bool IsActive, int ItemCount, DateTime CreatedDate);
}
