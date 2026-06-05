using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Endpoints;

public sealed partial class AddLookupSetEndpoint
{
    public sealed record Request(string Code, Localized? Descriptions);
}
