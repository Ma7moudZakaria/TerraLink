using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Mvc;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Endpoints;

public sealed partial class GetAllLookupSetsEndpoint
{
    public sealed class Request : PagedQueryBase
    {
        [FromQuery(Name = "code")] public string? Code { get; set; }
        [FromQuery(Name = "searchTerm")] public string? SearchTerm { get; set; }
        [FromQuery(Name = "isActive")] public bool? IsActive { get; set; }
    }
}
