using LowCodeHub.QueryableExtensions.Entities;
using TerraLink.Domain.ValueObjects;

namespace TerraLink.Domain.Entities
{
    public class AttachmentItemEntity : TrackedBaseEntity<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public AttachmentExtension Extension { get; set; }
        public string MimeType { get; set; } = string.Empty;
        public Guid AttachmentTypeId { get; set; }
        public LookupItemEntity AttachmentType { get; set; } = default!;
    }
}
