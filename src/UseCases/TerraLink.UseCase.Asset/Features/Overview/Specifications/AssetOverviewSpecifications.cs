using LowCodeHub.QueryableExtensions.Specifications;
using TerraLink.Domain.Entities;

namespace TerraLink.UseCase.Asset.Features.Overview.Specifications;

public sealed class ActiveLandsOverviewSpec : ISpecification<LandEntity>
{
    public IQueryable<LandEntity> Where(IQueryable<LandEntity> query)
        => query.Where(entity => !entity.IsDeleted);
}

public sealed class ActiveBuildingsOverviewSpec : ISpecification<BuildingEntity>
{
    public IQueryable<BuildingEntity> Where(IQueryable<BuildingEntity> query)
        => query.Where(entity => !entity.IsDeleted);
}

public sealed class ActiveUnitsOverviewSpec : ISpecification<UnitEntity>
{
    public IQueryable<UnitEntity> Where(IQueryable<UnitEntity> query)
        => query.Where(entity => !entity.IsDeleted);
}

public sealed class UnitsWithoutPriceOverviewSpec : ISpecification<UnitEntity>
{
    public IQueryable<UnitEntity> Where(IQueryable<UnitEntity> query)
        => query.Where(entity => !entity.IsDeleted && entity.Price <= 0);
}

public sealed class SoldUnitContractsOverviewSpec : ISpecification<ContractEntity>
{
    public IQueryable<ContractEntity> Where(IQueryable<ContractEntity> query)
        => query.Where(entity => !entity.IsDeleted && entity.UnitId != null);
}
