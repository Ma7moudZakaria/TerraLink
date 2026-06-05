namespace TerraLink.UseCase.Finance.Features.Overview.Operations;

public sealed partial class GetFinanceOverviewOperation
{
    public sealed record MonthlyInstallmentsPointResponse(int Month, decimal DueAmount, decimal PaidAmount);

    public sealed record UrgentActionResponse(
        Guid ContractId,
        string ContractNumber,
        Guid InstallmentId,
        string ClientName,
        DateTime DueDate,
        decimal RemainingAmount,
        string Status);

    public sealed record Response(
        decimal IncomingPaymentsThisMonth,
        decimal IncomingPaymentsThisYear,
        decimal DueInstallmentsThisMonth,
        decimal PaidInstallmentsThisMonth,
        decimal OverdueInstallmentsAmount,
        decimal CollectionRatePercentage,
        List<MonthlyInstallmentsPointResponse> MonthlyInstallments,
        List<UrgentActionResponse> UrgentActions);
}
