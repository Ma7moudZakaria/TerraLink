using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Asset.Features.Buildings.Endpoints
{
    public sealed partial class GetBuildingDetailsEndpoint
    {
        public sealed class Response
        {
            public required Guid Id { get; set; }
            public required string Name { get; set; }
            public required string Number { get; set; }
            public required string LandName { get; set; }
            public required int NumberOfFloors { get; set; }
            public required int NumberOfUnits { get; set; }
            public DateTime ConstructionYear { get; set; }
            public required Localized BuildingStatus { get; set; }
            public required decimal Length { get; set; }
            public required decimal Width { get; set; }
            public string? Description { get; set; }
            public AttributesDictionary? Attachments { get; set; }
        }
    }
}
