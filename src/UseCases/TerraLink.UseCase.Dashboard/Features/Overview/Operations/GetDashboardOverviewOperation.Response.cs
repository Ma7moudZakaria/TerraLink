namespace TerraLink.UseCase.Dashboard.Features.Overview.Operations;

public sealed partial class GetDashboardOverviewOperation
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
