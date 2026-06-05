using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Specifications;
using LowCodeHub.QueryableExtensions.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Units.Endpoints;

namespace TerraLink.UseCase.Asset.Features.Units.Specifications;

public sealed class CreateUnitAddSpec(CreateUnitEndpoint.Request request, string number) : IAddSpecification<UnitEntity>
{
    public UnitEntity Add()
    {
        return ObjectMapper.Map<CreateUnitEndpoint.Request, UnitEntity>(request)
            .Set(entity => entity.Id, Guid.CreateVersion7())
            .Set(entity => entity.Number, number)
            .Set(entity => entity.CreatedBy, "SYS")
            .Set(entity => entity.CreatedDate, DateTime.UtcNow);
    }
}

public sealed class SoftDeleteUnitUpdateSpec : IUpdateSpecification<UnitEntity>
{
    public Action<UpdateSettersBuilder<UnitEntity>> Update()
    {
        return setters => setters.SetProperty(entity => entity.IsDeleted, true);
    }
}

public sealed class UnitByIdSpec(Guid id) : ISpecification<UnitEntity>
{
    public IQueryable<UnitEntity> Where(IQueryable<UnitEntity> query)
    {
        return query.Where(entity => entity.Id == id);
    }
}

public sealed class UnitsCreatedInYearSpec(int year) : ISpecification<UnitEntity>
{
    public IQueryable<UnitEntity> Where(IQueryable<UnitEntity> query)
    {
        return query.Where(entity => entity.CreatedDate.Year == year);
    }
}

public sealed class UpdateUnitFieldsSpec : IUpdateSpecification<UnitEntity>
{
    public required string Name { get; init; }
    public required Guid BuildingId { get; init; }
    public Guid UnitStatusId { get; init; }
    public required int FloorNumber { get; init; }
    public required decimal Area { get; init; }
    public required int NumberOfRooms { get; init; }
    public required int NumberOfBatEmployeeooms { get; init; }
    public Guid UnitTypeId { get; init; }
    public required decimal Price { get; init; }
    public Guid FinishingTypeId { get; init; }
    public bool? HasBalcony { get; init; }
    public bool? HasGarage { get; init; }
    public bool? HasCentralAC { get; init; }
    public string? Description { get; init; }
    public AttributesDictionary? Attachments { get; init; }

    public Action<UpdateSettersBuilder<UnitEntity>> Update()
    {
        return setters => setters
            .SetProperty(entity => entity.Name, Name)
            .SetProperty(entity => entity.BuildingId, BuildingId)
            .SetProperty(entity => entity.UnitStatusId, UnitStatusId)
            .SetProperty(entity => entity.FloorNumber, FloorNumber)
            .SetProperty(entity => entity.Area, Area)
            .SetProperty(entity => entity.NumberOfRooms, NumberOfRooms)
            .SetProperty(entity => entity.NumberOfBathrooms, NumberOfBatEmployeeooms)
            .SetProperty(entity => entity.UnitTypeId, UnitTypeId)
            .SetProperty(entity => entity.Price, Price)
            .SetProperty(entity => entity.FinishingTypeId, FinishingTypeId)
            .SetProperty(entity => entity.HasBalcony, HasBalcony)
            .SetProperty(entity => entity.HasGarage, HasGarage)
            .SetProperty(entity => entity.HasCentralAC, HasCentralAC)
            .SetProperty(entity => entity.Description, Description)
            .SetProperty(entity => entity.Attachments, Attachments);
    }
}

public sealed class UnitDetailsSpec(Guid id) : ISpecification<UnitEntity>
{
    public IQueryable<UnitEntity> Where(IQueryable<UnitEntity> query)
    {
        return query
            .Include(entity => entity.Building)
                .ThenInclude(building => building.Land)
            .Include(entity => entity.UnitType)
            .Include(entity => entity.UnitStatus)
            .Include(entity => entity.FinishingType)
            .Include(entity => entity.Contracts)
                .ThenInclude(contract => contract.Client)
            .Include(entity => entity.Contracts)
                .ThenInclude(contract => contract.Installments)
            .Where(entity => entity.Id == id && !entity.IsDeleted);
    }
}

public sealed class UnitsListSpec(GetUnitsEndpoint.Request request) : ISpecification<UnitEntity>
{
    public IQueryable<UnitEntity> Where(IQueryable<UnitEntity> query)
    {
        query = query
            .Include(entity => entity.Building)
                .ThenInclude(building => building.Land)
            .Include(entity => entity.UnitStatus)
            .Where(entity => !entity.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Number))
        {
            query = query.Where(entity => entity.Number.Contains(request.Number));
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(entity => entity.Name.Contains(request.Name));
        }

        if (request.BuildingId.HasValue)
        {
            query = query.Where(entity => entity.BuildingId == request.BuildingId.Value);
        }

        if (request.LandId.HasValue)
        {
            query = query.Where(entity => entity.Building.LandId == request.LandId.Value);
        }

        if (request.StatusId.HasValue)
        {
            query = query.Where(entity => entity.UnitStatusId == request.StatusId.Value);
        }

        if (request.FloorNumber.HasValue)
        {
            query = query.Where(entity => entity.FloorNumber == request.FloorNumber.Value);
        }

        if (request.Area.HasValue)
        {
            query = query.Where(entity => entity.Area == request.Area.Value);
        }

        if (request.Price.HasValue)
        {
            query = query.Where(entity => entity.Price == request.Price.Value);
        }

        if (request.NumberOfRooms.HasValue)
        {
            query = query.Where(entity => entity.NumberOfRooms == request.NumberOfRooms.Value);
        }

        return query.OrderBy(entity => entity.Number);
    }
}

public sealed class ActiveUnitsSpec(Guid? buildingId, Guid? landId) : ISpecification<UnitEntity>
{
    public IQueryable<UnitEntity> Where(IQueryable<UnitEntity> query)
    {
        query = query.Where(entity => !entity.IsDeleted);

        if (buildingId.HasValue)
        {
            query = query.Where(entity => entity.BuildingId == buildingId.Value);
        }

        if (landId.HasValue)
        {
            query = query.Where(entity => entity.Building.LandId == landId.Value);
        }

        return query.OrderBy(entity => entity.Name);
    }
}
