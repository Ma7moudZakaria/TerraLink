using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Asset.Features.Units.Endpoints
{
    public sealed partial class CreateUnitEndpoint
    {
        public sealed class Request
        {
            public required string Name { get; set; }
            public required Guid BuildingId { get; set; }
            public Guid UnitStatusId { get; set; }
            public required int FloorNumber { get; set; }
            public required decimal Area { get; set; }
            public required int NumberOfRooms { get; set; }
            public required int NumberOfBathrooms { get; set; }
            public Guid UnitTypeId { get; set; }
            public required decimal Price { get; set; }
            public Guid FinishingTypeId { get; set; }
            public bool? HasBalcony { get; set; }
            public bool? HasGarage { get; set; }
            public bool? HasCentralAC { get; set; }
            public string? Description { get; set; }
            public AttributesDictionary? Attachments { get; set; }
        }
    }
}
