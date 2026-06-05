using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Mvc;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Endpoints;

public sealed partial class GetLookupItemsBySetIdEndpoint
{
    public sealed class Request : PagedQueryBase
    {
        [FromRoute] public required Guid LookupSetId { get; set; }
        [FromQuery] public string? Code { get; set; }
        [FromQuery] public string? SearchTerm { get; set; }
        [FromQuery] public bool? IsActive { get; set; }
    }
}
