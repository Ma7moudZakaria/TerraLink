namespace TerraLink.UseCase.Asset.Features.Overview.Operations
{
    public sealed partial class GetAssetOverviewOperation
    {
        public sealed class Response
        {
            public int TotalLands { get; set; }
            public int TotalBuildings { get; set; }
            public int TotalUnits { get; set; }
            public decimal TotalLandArea { get; set; }
            public decimal TotalBuildingArea { get; set; }
            public int SoldUnits { get; set; }
            public int AvailableUnits { get; set; }
            public int UnitsWithoutPrice { get; set; }
        }
    }
}
