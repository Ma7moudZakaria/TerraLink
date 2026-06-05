using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.Domain.Entities
{
    public class BuildingEntity : TrackedBaseEntity<Guid>
    {
        public required string Name { get; set; }
        public string Number { get; set; } = string.Empty;

        public required Guid LandId { get; set; }
        public required int NumberOfFloors { get; set; }
        public required int NumberOfUnits { get; set; }
        public DateTime ConstructionYear { get; set; }
        public Guid BuildingStatusId { get; set; }
        public required decimal Length { get; set; }
        public required decimal Width { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }

        public AttributesDictionary? Attachments { get; set; }
        public LandEntity Land { get; set; } = default!;
        public LookupItemEntity BuildingStatus { get; set; } = default!;

        public ICollection<UnitEntity> Units { get; set; } = [];

    }
}
