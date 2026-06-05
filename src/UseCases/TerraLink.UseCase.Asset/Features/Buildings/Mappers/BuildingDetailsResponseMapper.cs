using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Buildings.Operations;

namespace TerraLink.UseCase.Asset.Features.Buildings.Mappers;

public sealed class BuildingDetailsResponseMapper : IMapHandler<BuildingEntity, GetBuildingDetailsOperation.Response>
{
    public GetBuildingDetailsOperation.Response Handler(BuildingEntity source)
    {
        return new GetBuildingDetailsOperation.Response
        {
            Id = source.Id,
            Name = source.Name,
            Number = source.Number,
            LandName = source.Land.Name,
            NumberOfFloors = source.NumberOfFloors,
            NumberOfUnits = source.NumberOfUnits,
            ConstructionYear = source.ConstructionYear,
            BuildingStatus = source.BuildingStatus.Descriptions,
            Length = source.Length,
            Width = source.Width,
            Description = source.Description,
            Attachments = source.Attachments
        };
    }
}
