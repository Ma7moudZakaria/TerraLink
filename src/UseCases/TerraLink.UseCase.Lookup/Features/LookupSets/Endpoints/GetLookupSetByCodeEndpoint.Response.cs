using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Endpoints;

public sealed partial class GetLookupSetByCodeEndpoint
{
    public sealed class ItemResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public Localized Descriptions { get; set; } = default!;
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
    }

    public sealed class Response
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public Localized Descriptions { get; set; } = default!;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public List<ItemResponse> Items { get; set; } = [];
    }
}
