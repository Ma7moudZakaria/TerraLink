using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Lands.Operations;

namespace TerraLink.UseCase.Asset.Features.Lands.Mappers;

public sealed class LandListResponseMapper : IMapHandler<LandEntity, GetLandsOperation.Response>
{
    public GetLandsOperation.Response Handler(LandEntity source)
    {
        return new GetLandsOperation.Response
        {
            Id = source.Id,
            Number = source.Number,
            Name = source.Name,
            District = source.District.Descriptions,
            City = source.City.Descriptions,
            Governorate = source.Governorate.Descriptions,
            Area = source.Length * source.Width,
            BuildingsCount = source.Buildings.Count,
            UnitsCount = source.Buildings.Sum(building => building.NumberOfUnits),
            CreationDate = source.CreatedDate
        };
    }
}
