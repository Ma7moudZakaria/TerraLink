using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Asset.Features.Buildings.Operations
{
    public sealed partial class GetBuildingsOperation
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
            public DateTime CreationDate { get; set; }
            public required Localized BuildingStatus { get; set; }
            public required decimal Area { get; set; }
        }
    }
}
