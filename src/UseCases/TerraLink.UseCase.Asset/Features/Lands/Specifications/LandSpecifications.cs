using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Specifications;
using LowCodeHub.QueryableExtensions.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Lands.Endpoints;

namespace TerraLink.UseCase.Asset.Features.Lands.Specifications;

public sealed class CreateLandAddSpec(CreateLandEndpoint.Request request, string number) : IAddSpecification<LandEntity>
{
    public LandEntity Add()
    {
        return ObjectMapper.Map<CreateLandEndpoint.Request, LandEntity>(request)
            .Set(entity => entity.Id, Guid.CreateVersion7())
            .Set(entity => entity.Number, number)
            .Set(entity => entity.CreatedBy, "SYS")
            .Set(entity => entity.CreatedDate, DateTime.UtcNow);
    }
}

public sealed class SoftDeleteLandUpdateSpec : IUpdateSpecification<LandEntity>
{
    public Action<UpdateSettersBuilder<LandEntity>> Update()
    {
        return setters => setters.SetProperty(entity => entity.IsDeleted, true);
    }
}

public sealed class LandByIdSpec(Guid id) : ISpecification<LandEntity>
{
    public IQueryable<LandEntity> Where(IQueryable<LandEntity> query)
    {
        return query.Where(entity => entity.Id == id);
    }
}

public sealed class LandsCreatedInYearSpec(int year) : ISpecification<LandEntity>
{
    public IQueryable<LandEntity> Where(IQueryable<LandEntity> query)
    {
        return query.Where(entity => entity.CreatedDate.Year == year);
    }
}

public sealed class UpdateLandFieldsSpec : IUpdateSpecification<LandEntity>
{
    public required string Name { get; init; }
    public Guid GovernorateId { get; init; }
    public Guid CityId { get; init; }
    public Guid DistrictId { get; init; }
    public required decimal Length { get; init; }
    public required decimal Width { get; init; }
    public decimal? Latitude { get; init; }
    public decimal? Longitude { get; init; }
    public string? Description { get; init; }
    public AttributesDictionary? Attachments { get; init; }

    public Action<UpdateSettersBuilder<LandEntity>> Update()
    {
        return setters => setters
            .SetProperty(entity => entity.Name, Name)
            .SetProperty(entity => entity.GovernorateId, GovernorateId)
            .SetProperty(entity => entity.CityId, CityId)
            .SetProperty(entity => entity.DistrictId, DistrictId)
            .SetProperty(entity => entity.Length, Length)
            .SetProperty(entity => entity.Width, Width)
            .SetProperty(entity => entity.Latitude, Latitude)
            .SetProperty(entity => entity.Longitude, Longitude)
            .SetProperty(entity => entity.Description, Description)
            .SetProperty(entity => entity.Attachments, Attachments);
    }
}

public sealed class LandDetailsSpec(Guid id) : ISpecification<LandEntity>
{
    public IQueryable<LandEntity> Where(IQueryable<LandEntity> query)
    {
        return query
            .Include(entity => entity.Governorate)
            .Include(entity => entity.City)
            .Include(entity => entity.District)
            .Where(entity => entity.Id == id && !entity.IsDeleted);
    }
}

public sealed class LandsListSpec(GetLandsEndpoint.Request request) : ISpecification<LandEntity>
{
    public IQueryable<LandEntity> Where(IQueryable<LandEntity> query)
    {
        query = query
            .Include(entity => entity.Governorate)
            .Include(entity => entity.City)
            .Include(entity => entity.District)
            .Include(entity => entity.Buildings)
            .Where(entity => !entity.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Number))
        {
            query = query.Where(entity => entity.Number.Contains(request.Number));
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(entity => entity.Name.Contains(request.Name));
        }

        if (request.GovernorateId.HasValue)
        {
            query = query.Where(entity => entity.GovernorateId == request.GovernorateId.Value);
        }

        if (request.CityId.HasValue)
        {
            query = query.Where(entity => entity.CityId == request.CityId.Value);
        }

        if (request.DistrictId.HasValue)
        {
            query = query.Where(entity => entity.DistrictId == request.DistrictId.Value);
        }

        if (request.Area.HasValue)
        {
            query = query.Where(entity => entity.Length * entity.Width == request.Area.Value);
        }

        if (request.ConstructionDate.HasValue)
        {
            query = query.Where(entity => entity.CreatedDate.Date == request.ConstructionDate.Value.Date);
        }

        if (request.BuildingCount.HasValue)
        {
            query = query.Where(entity => entity.Buildings.Count == request.BuildingCount.Value);
        }

        if (request.UnitCount.HasValue)
        {
            query = query.Where(entity => entity.Buildings.Sum(building => building.NumberOfUnits) == request.UnitCount.Value);
        }

        return query.OrderBy(entity => entity.Number);
    }
}

public sealed class ActiveLandsSpec : ISpecification<LandEntity>
{
    public IQueryable<LandEntity> Where(IQueryable<LandEntity> query)
    {
        return query
            .Where(entity => !entity.IsDeleted)
            .OrderBy(entity => entity.Name);
    }
}
