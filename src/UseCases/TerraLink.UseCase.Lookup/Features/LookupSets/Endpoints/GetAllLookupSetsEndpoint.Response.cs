using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Endpoints;

public sealed partial class GetAllLookupSetsEndpoint
{
    public sealed record Response(Guid Id, string Code, Localized Descriptions, bool IsActive, int ItemCount, DateTime CreatedDate);
}
