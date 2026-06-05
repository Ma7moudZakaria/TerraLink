using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Asset.Features.Lands.Endpoints
{
    public sealed partial class CreateLandEndpoint
    {
        public sealed class Request
        {
            public required string Name { get; set; }
            public Guid GovernorateId { get; set; }
            public Guid CityId { get; set; }
            public Guid DistrictId { get; set; }
            public required decimal Length { get; set; }
            public required decimal Width { get; set; }
            public decimal? Latitude { get; set; }
            public decimal? Longitude { get; set; }
            public string? Description { get; set; }
            public AttributesDictionary? Attachments { get; set; }
        }
    }
}
