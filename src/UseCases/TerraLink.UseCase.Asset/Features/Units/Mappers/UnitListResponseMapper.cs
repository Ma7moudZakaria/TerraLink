using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Units.Operations;

namespace TerraLink.UseCase.Asset.Features.Units.Mappers;

public sealed class UnitListResponseMapper : IMapHandler<UnitEntity, GetUnitsOperation.Response>
{
    public GetUnitsOperation.Response Handler(UnitEntity source)
    {
        return new GetUnitsOperation.Response
        {
            Id = source.Id,
            Name = source.Name,
            Number = source.Number,
            Building = source.Building.Name,
            Land = source.Building.Land.Name,
            FloorNumber = source.FloorNumber,
            Area = source.Area,
            NumberOfRooms = source.NumberOfRooms,
            NumberOfBatEmployeeooms = source.NumberOfBathrooms,
            Price = source.Price,
            Status = source.UnitStatus.Descriptions
        };
    }
}
