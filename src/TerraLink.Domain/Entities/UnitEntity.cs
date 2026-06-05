using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.Domain.Entities
{
    public class UnitEntity : TrackedBaseEntity<Guid>
    {
        public required string Name { get; set; }
        public string Number { get; set; } = string.Empty;
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
        public bool IsDeleted { get; set; }

        public AttributesDictionary? Attachments { get; set; }
        public BuildingEntity Building { get; set; } = default!;
        public LookupItemEntity UnitType { get; set; } = default!;
        public LookupItemEntity UnitStatus { get; set; } = default!;
        public LookupItemEntity FinishingType { get; set; } = default!;
        public ICollection<ContractEntity> Contracts { get; set; } = [];


    }
}
