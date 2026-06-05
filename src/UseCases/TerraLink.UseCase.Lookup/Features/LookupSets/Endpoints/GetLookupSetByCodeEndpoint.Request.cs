using Microsoft.AspNetCore.Mvc;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Endpoints;

public sealed partial class GetLookupSetByCodeEndpoint
{
    public sealed class Request
    {
        [FromRoute] public required string Code { get; set; }
    }
}
