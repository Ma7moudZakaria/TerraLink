using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Mvc;

namespace TerraLink.UseCase.Sales.Features.Clients.Endpoints
{
    public sealed partial class GetAllClientsEndpoint
    {
        public sealed class Request : PagedQueryBase
        {
            [FromQuery] public string? UnitNumber { get; set; }
            [FromQuery] public string? BuildingName { get; set; }
            [FromQuery] public string? LandName { get; set; }
            [FromQuery] public int? FloorNumber { get; set; }
        }
    }
}
