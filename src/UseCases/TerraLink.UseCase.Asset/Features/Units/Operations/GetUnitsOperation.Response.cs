using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Asset.Features.Units.Operations
{
    public sealed partial class GetUnitsOperation
    {
        public sealed class Response
        {
            public required Guid Id { get; set; }
            public required string Name { get; set; }
            public required string Number { get; set; }
            public required string Building { get; set; }
            public required string Land { get; set; }
            public required int FloorNumber { get; set; }
            public required decimal Area { get; set; }
            public required int NumberOfRooms { get; set; }
            public required int NumberOfBatEmployeeooms { get; set; }
            public required decimal Price { get; set; }
            public required Localized Status { get; set; }
        }
    }
}
