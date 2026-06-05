using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Asset.Features.Buildings.Endpoints
{
    public sealed partial class UpdateBuildingEndpoint
    {
        public sealed class Request
        {
            public required string Name { get; set; }
            public required Guid LandId { get; set; }
            public required int NumberOfFloors { get; set; }
            public required int NumberOfUnits { get; set; }
            public DateTime ConstructionYear { get; set; }
            public Guid BuildingStatusId { get; set; }
            public required decimal Length { get; set; }
            public required decimal Width { get; set; }
            public string? Description { get; set; }
            public AttributesDictionary? Attachments { get; set; }
        }
    }
}
