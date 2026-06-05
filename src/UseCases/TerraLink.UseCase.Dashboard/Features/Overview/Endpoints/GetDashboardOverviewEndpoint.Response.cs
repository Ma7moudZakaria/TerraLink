namespace TerraLink.UseCase.Dashboard.Features.Overview.Endpoints;

public sealed partial class GetDashboardOverviewEndpoint
{
    public sealed record Response(
        int TotalContracts,
        decimal TotalSalesValue,
        decimal IncomingPaymentsThisMonth,
        decimal TotalExpensesThisMonth,
        int NewCustomersThisMonth,
        decimal DueInstallmentsThisMonth,
        decimal CollectionRatePercentage,
        decimal NetProfitThisMonth);
}
