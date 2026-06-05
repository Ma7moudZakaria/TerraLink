using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Mvc;

namespace TerraLink.UseCase.Asset.Features.Units.Endpoints
{
    public sealed partial class GetUnitsEndpoint
    {
        public sealed class Request : PagedQueryBase
        {
            [FromQuery] public string? Number { get; set; }
            [FromQuery] public string? Name { get; set; }
            [FromQuery] public Guid? BuildingId { get; set; }
            [FromQuery] public Guid? LandId { get; set; }
            [FromQuery] public Guid? StatusId { get; set; }
            [FromQuery] public int? FloorNumber { get; set; }
            [FromQuery] public decimal? Area { get; set; }
            [FromQuery] public decimal? Price { get; set; }
            [FromQuery] public int? NumberOfRooms { get; set; }
        }
    }
}
