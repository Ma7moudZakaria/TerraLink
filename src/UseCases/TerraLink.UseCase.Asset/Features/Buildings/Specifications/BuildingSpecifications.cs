using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Specifications;
using LowCodeHub.QueryableExtensions.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Buildings.Endpoints;

namespace TerraLink.UseCase.Asset.Features.Buildings.Specifications;

public sealed class CreateBuildingAddSpec(CreateBuildingEndpoint.Request request, string number) : IAddSpecification<BuildingEntity>
{
    public BuildingEntity Add()
    {
        return ObjectMapper.Map<CreateBuildingEndpoint.Request, BuildingEntity>(request)
            .Set(entity => entity.Id, Guid.CreateVersion7())
            .Set(entity => entity.Number, number)
            .Set(entity => entity.CreatedBy, "SYS")
            .Set(entity => entity.CreatedDate, DateTime.UtcNow);
    }
}

public sealed class SoftDeleteBuildingUpdateSpec : IUpdateSpecification<BuildingEntity>
{
    public Action<UpdateSettersBuilder<BuildingEntity>> Update()
    {
        return setters => setters.SetProperty(entity => entity.IsDeleted, true);
    }
}

public sealed class BuildingByIdSpec(Guid id) : ISpecification<BuildingEntity>
{
    public IQueryable<BuildingEntity> Where(IQueryable<BuildingEntity> query)
    {
        return query.Where(entity => entity.Id == id);
    }
}

public sealed class BuildingsCreatedInYearSpec(int year) : ISpecification<BuildingEntity>
{
    public IQueryable<BuildingEntity> Where(IQueryable<BuildingEntity> query)
    {
        return query.Where(entity => entity.CreatedDate.Year == year);
    }
}

public sealed class UpdateBuildingFieldsSpec : IUpdateSpecification<BuildingEntity>
{
    public required string Name { get; init; }
    public required Guid LandId { get; init; }
    public required int NumberOfFloors { get; init; }
    public required int NumberOfUnits { get; init; }
    public DateTime ConstructionYear { get; init; }
    public Guid BuildingStatusId { get; init; }
    public required decimal Length { get; init; }
    public required decimal Width { get; init; }
    public string? Description { get; init; }
    public AttributesDictionary? Attachments { get; init; }

    public Action<UpdateSettersBuilder<BuildingEntity>> Update()
    {
        return setters => setters
            .SetProperty(entity => entity.Name, Name)
            .SetProperty(entity => entity.LandId, LandId)
            .SetProperty(entity => entity.NumberOfFloors, NumberOfFloors)
            .SetProperty(entity => entity.NumberOfUnits, NumberOfUnits)
            .SetProperty(entity => entity.Length, Length)
            .SetProperty(entity => entity.Width, Width)
            .SetProperty(entity => entity.ConstructionYear, ConstructionYear)
            .SetProperty(entity => entity.BuildingStatusId, BuildingStatusId)
            .SetProperty(entity => entity.Description, Description)
            .SetProperty(entity => entity.Attachments, Attachments);
    }
}

public sealed class BuildingDetailsSpec(Guid id) : ISpecification<BuildingEntity>
{
    public IQueryable<BuildingEntity> Where(IQueryable<BuildingEntity> query)
    {
        return query
            .Include(entity => entity.Land)
            .Include(entity => entity.BuildingStatus)
            .Where(entity => entity.Id == id && !entity.IsDeleted);
    }
}

public sealed class BuildingsListSpec(GetBuildingsEndpoint.Request request) : ISpecification<BuildingEntity>
{
    public IQueryable<BuildingEntity> Where(IQueryable<BuildingEntity> query)
    {
        query = query
            .Include(entity => entity.Land)
            .Include(entity => entity.BuildingStatus)
            .Where(entity => !entity.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Number))
        {
            query = query.Where(entity => entity.Number.Contains(request.Number));
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(entity => entity.Name.Contains(request.Name));
        }

        if (request.LandId.HasValue)
        {
            query = query.Where(entity => entity.LandId == request.LandId.Value);
        }

        if (request.StatusId.HasValue)
        {
            query = query.Where(entity => entity.BuildingStatusId == request.StatusId.Value);
        }

        if (request.ConstructionDate.HasValue)
        {
            query = query.Where(entity => entity.CreatedDate.Date == request.ConstructionDate.Value.Date);
        }

        if (request.Area.HasValue)
        {
            query = query.Where(entity => entity.Length * entity.Width == request.Area.Value);
        }

        if (request.FloorCount.HasValue)
        {
            query = query.Where(entity => entity.NumberOfFloors == request.FloorCount.Value);
        }

        if (request.UnitCount.HasValue)
        {
            query = query.Where(entity => entity.NumberOfUnits == request.UnitCount.Value);
        }

        return query.OrderBy(entity => entity.Number);
    }
}

public sealed class ActiveBuildingsSpec(Guid? landId) : ISpecification<BuildingEntity>
{
    public IQueryable<BuildingEntity> Where(IQueryable<BuildingEntity> query)
    {
        query = query.Where(entity => !entity.IsDeleted);

        if (landId.HasValue)
        {
            query = query.Where(entity => entity.LandId == landId.Value);
        }

        return query.OrderBy(entity => entity.Name);
    }
}
