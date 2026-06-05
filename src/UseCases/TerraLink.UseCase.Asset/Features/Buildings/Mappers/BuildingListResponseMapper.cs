using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Buildings.Operations;

namespace TerraLink.UseCase.Asset.Features.Buildings.Mappers;

public sealed class BuildingListResponseMapper : IMapHandler<BuildingEntity, GetBuildingsOperation.Response>
{
    public GetBuildingsOperation.Response Handler(BuildingEntity source)
    {
        return new GetBuildingsOperation.Response
        {
            Id = source.Id,
            Name = source.Name,
            Number = source.Number,
            LandName = source.Land.Name,
            NumberOfFloors = source.NumberOfFloors,
            NumberOfUnits = source.NumberOfUnits,
            ConstructionYear = source.ConstructionYear,
            CreationDate = source.CreatedDate,
            BuildingStatus = source.BuildingStatus.Descriptions,
            Area = source.Length * source.Width
        };
    }
}
