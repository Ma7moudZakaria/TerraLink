using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.Domain.Entities
{
    public class LandEntity : TrackedBaseEntity<Guid>
    {
        public required string Name { get; set; }
        public Guid GovernorateId { get; set; }
        public string Number { get; set; } = string.Empty;
        public Guid CityId { get; set; }
        public Guid DistrictId { get; set; }
        public required decimal Length { get; set; }
        public required decimal Width { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? Description { get; set; }

        public bool IsDeleted { get; set; }
        /// <summary>
        ///  [
        /// {
        ///    "ComponentId" : "building_linc",
        ///     "AttachmentId" : "xyz"
        /// }            
        ///]
        /// </summary>
        public AttributesDictionary? Attachments { get; set; }
        public LookupItemEntity Governorate { get; set; } = default!;
        public LookupItemEntity City { get; set; } = default!;
        public LookupItemEntity District { get; set; } = default!;

        public ICollection<BuildingEntity> Buildings { get; set; } = [];

    }
}
