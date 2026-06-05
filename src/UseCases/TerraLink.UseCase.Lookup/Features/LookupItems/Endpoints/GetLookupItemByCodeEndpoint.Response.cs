using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Endpoints;

public sealed partial class GetLookupItemByCodeEndpoint
{
    public sealed class LookupSetResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public Localized Descriptions { get; set; } = default!;
    }

    public sealed class Response
    {
        public Guid Id { get; set; }
        public Guid LookupSetId { get; set; }
        public string Code { get; set; } = string.Empty;
        public Localized Descriptions { get; set; } = default!;
        public AttributeDictionary? Metadata { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public LookupSetResponse? LookupSet { get; set; }
    }
}
