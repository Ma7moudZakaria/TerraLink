using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Lands.Operations;

namespace TerraLink.UseCase.Asset.Features.Lands.Mappers;

public sealed class LandDetailsResponseMapper : IMapHandler<LandEntity, GetLandDetailsOperation.Response>
{
    public GetLandDetailsOperation.Response Handler(LandEntity source)
    {
        return new GetLandDetailsOperation.Response
        {
            Name = source.Name,
            GovernorateName = source.Governorate.Descriptions,
            CityName = source.City.Descriptions,
            DistrictName = source.District.Descriptions,
            Length = source.Length,
            Width = source.Width,
            Latitude = source.Latitude,
            Longitude = source.Longitude,
            Description = source.Description,
            Attachments = source.Attachments
        };
    }
}
