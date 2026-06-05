using Microsoft.AspNetCore.Mvc;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Endpoints;

public sealed partial class GetLookupItemByCodeEndpoint
{
    public sealed class Request
    {
        [FromRoute] public required string SetCode { get; set; }
        [FromRoute] public required string ItemCode { get; set; }
    }
}
