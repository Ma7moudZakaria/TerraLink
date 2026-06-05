using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Mvc;

namespace TerraLink.UseCase.Asset.Features.Buildings.Endpoints
{
    public sealed partial class GetBuildingsEndpoint
    {
        public sealed class Request : PagedQueryBase
        {
            [FromQuery] public string? Number { get; set; }
            [FromQuery] public string? Name { get; set; }
            [FromQuery] public Guid? LandId { get; set; }
            [FromQuery] public Guid? StatusId { get; set; }
            [FromQuery] public DateTime? ConstructionDate { get; set; }
            [FromQuery] public decimal? Area { get; set; }
            [FromQuery] public int? FloorCount { get; set; }
            [FromQuery] public int? UnitCount { get; set; }
        }
    }
}
