using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Mvc;

namespace TerraLink.UseCase.Asset.Features.Lands.Endpoints
{
    public sealed partial class GetLandsEndpoint
    {
        public sealed class Request : PagedQueryBase
        {
            [FromQuery] public string? Number { get; set; }
            [FromQuery] public string? Name { get; set; }
            [FromQuery] public Guid? GovernorateId { get; set; }
            [FromQuery] public Guid? CityId { get; set; }
            [FromQuery] public Guid? DistrictId { get; set; }
            [FromQuery] public decimal? Area { get; set; }
            [FromQuery] public DateTime? ConstructionDate { get; set; }
            [FromQuery] public int? BuildingCount { get; set; }
            [FromQuery] public int? UnitCount { get; set; }
        }
    }
}
