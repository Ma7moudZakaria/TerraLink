using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Overview.Specifications;

namespace TerraLink.UseCase.Asset.Features.Overview.Operations;

public sealed partial class GetAssetOverviewOperation(
    IRepository<LandEntity> lands,
    IRepository<BuildingEntity> buildings,
    IRepository<UnitEntity> units,
    IRepository<ContractEntity> contracts)
    : IOperationHandler<GetAssetOverviewOperation.Request, GetAssetOverviewOperation.Response>
{
    public async Task<ErrorOr<GetAssetOverviewOperation.Response>> HandleAsync(
        GetAssetOverviewOperation.Request request,
        CancellationToken ct = default)
    {
        IReadOnlyList<LandEntity> activeLands = await lands.ListAsync(new ActiveLandsOverviewSpec(), ct);
        IReadOnlyList<BuildingEntity> activeBuildings = await buildings.ListAsync(new ActiveBuildingsOverviewSpec(), ct);

        int totalUnits = await units.CountAsync(new ActiveUnitsOverviewSpec(), ct);
        int unitsWithoutPrice = await units.CountAsync(new UnitsWithoutPriceOverviewSpec(), ct);

        IReadOnlyList<ContractEntity> soldContracts = await contracts.ListAsync(new SoldUnitContractsOverviewSpec(), ct);
        int soldUnits = soldContracts.Select(contract => contract.UnitId!.Value).Distinct().Count();

        return new GetAssetOverviewOperation.Response
        {
            TotalLands = activeLands.Count,
            TotalBuildings = activeBuildings.Count,
            TotalUnits = totalUnits,
            TotalLandArea = activeLands.Sum(land => land.Length * land.Width),
            TotalBuildingArea = activeBuildings.Sum(building => building.Length * building.Width),
            SoldUnits = soldUnits,
            AvailableUnits = Math.Max(0, totalUnits - soldUnits),
            UnitsWithoutPrice = unitsWithoutPrice
        };
    }
}
