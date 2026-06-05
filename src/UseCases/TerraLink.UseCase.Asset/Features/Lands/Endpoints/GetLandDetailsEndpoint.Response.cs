using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Asset.Features.Lands.Endpoints
{
    public sealed partial class GetLandDetailsEndpoint
    {
        public sealed class Response
        {
            public required string Name { get; set; }
            public required Localized GovernorateName { get; set; }
            public required Localized CityName { get; set; }
            public required Localized DistrictName { get; set; }
            public required decimal Length { get; set; }
            public required decimal Width { get; set; }
            public decimal? Latitude { get; set; }
            public decimal? Longitude { get; set; }
            public string? Description { get; set; }
            public AttributesDictionary? Attachments { get; set; }
        }
    }
}
