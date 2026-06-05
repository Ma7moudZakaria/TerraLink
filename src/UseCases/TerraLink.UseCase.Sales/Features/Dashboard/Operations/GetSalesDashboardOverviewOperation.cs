using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Shared.Operations;
using TerraLink.UseCase.Sales.Features.Shared.Specifications;

namespace TerraLink.UseCase.Sales.Features.Dashboard.Operations;

public sealed partial class GetSalesDashboardOverviewOperation(
    IRepository<ContractEntity> contracts,
    IRepository<IncomingPaymentEntity> incomingPayments,
    IRepository<ClientEntity> clients,
    IRepository<ContractInstallmentEntity> installments)
    : IOperationHandler<GetSalesDashboardOverviewOperation.Request, GetSalesDashboardOverviewOperation.Response>
{
    public async Task<ErrorOr<GetSalesDashboardOverviewOperation.Response>> HandleAsync(
        GetSalesDashboardOverviewOperation.Request request,
        CancellationToken ct = default)
    {
        DateTime utcNow = DateTime.UtcNow;
        DateTime monthStart = new(utcNow.Year, utcNow.Month, 1);
        DateTime monthEnd = monthStart.AddMonths(1);
        DateTime today = utcNow.Date;
        DateTime dueSoonLimit = today.AddDays(7);

        IReadOnlyList<ContractEntity> activeContracts = await contracts.ListAsync(new ActiveSalesContractsOverviewSpec(), ct);
        IReadOnlyList<IncomingPaymentEntity> activeIncomingPayments = await incomingPayments.ListAsync(new ActiveSalesIncomingPaymentsOverviewSpec(), ct);
        IReadOnlyList<ClientEntity> activeClients = await clients.ListAsync(new ActiveSalesClientsOverviewSpec(), ct);

        List<SalesInstallmentStatusRow> allInstallmentRows = await SalesOverviewReadModels.GetInstallmentStatusRowsAsync(installments, incomingPayments, ct);
        List<SalesInstallmentStatusRow> unpaidInstallmentRows = allInstallmentRows.Where(row => row.RemainingAmount > 0m).ToList();

        List<GetSalesDashboardOverviewOperation.UrgentInstallmentActionResponse> urgentActions = unpaidInstallmentRows
            .Where(row => row.DueDate < dueSoonLimit)
            .OrderBy(row => row.DueDate)
            .Take(10)
            .Select(row => new GetSalesDashboardOverviewOperation.UrgentInstallmentActionResponse
            {
                InstallmentId = row.InstallmentId,
                ContractId = row.ContractId,
                ContractNumber = row.ContractNumber,
                ClientId = row.ClientId,
                ClientName = row.ClientName,
                DueDate = row.DueDate,
                InstallmentAmount = row.InstallmentAmount,
                PaidAmount = row.PaidAmount,
                RemainingAmount = row.RemainingAmount,
                DaysToDue = (row.DueDate.Date - today).Days,
                Status = row.DueDate.Date < today ? "overdue" : "due_soon"
            })
            .ToList();

        return new GetSalesDashboardOverviewOperation.Response
        {
            TotalContracts = activeContracts.Count,
            TotalSalesValue = activeContracts.Sum(contract => contract.TotalPrice),
            TotalIncomingPayments = activeIncomingPayments.Sum(payment => payment.Amount),
            InstallmentsDueThisMonth = unpaidInstallmentRows
                .Where(row => row.DueDate >= monthStart && row.DueDate < monthEnd)
                .Sum(row => row.RemainingAmount),
            InstallmentsOverdue = unpaidInstallmentRows
                .Where(row => row.DueDate < today)
                .Sum(row => row.RemainingAmount),
            CollectionRatePercentage = CalculateCollectionRate(allInstallmentRows),
            NewClientsThisMonth = activeClients.Count(client => client.CreatedDate >= monthStart && client.CreatedDate < monthEnd),
            UrgentActions = urgentActions,
            InstallmentStatusSummary = BuildDashboardInstallmentStatusSummary(allInstallmentRows, today, dueSoonLimit)
        };
    }

    private static decimal CalculateCollectionRate(List<SalesInstallmentStatusRow> installmentRows)
    {
        decimal totalInstallmentsAmount = installmentRows.Sum(row => row.InstallmentAmount);
        decimal paidInstallmentsAmount = installmentRows.Sum(row => row.PaidAmount);

        return totalInstallmentsAmount <= 0m
            ? 0m
            : Math.Round((paidInstallmentsAmount / totalInstallmentsAmount) * 100m, 2);
    }

    private static List<GetSalesDashboardOverviewOperation.DashboardInstallmentStatusSummaryResponse> BuildDashboardInstallmentStatusSummary(
        List<SalesInstallmentStatusRow> installmentRows,
        DateTime today,
        DateTime dueSoonLimit)
    {
        return
        [
            new GetSalesDashboardOverviewOperation.DashboardInstallmentStatusSummaryResponse
            {
                Status = "paid",
                Count = installmentRows.Count(row => row.RemainingAmount <= 0m),
                Amount = installmentRows.Where(row => row.RemainingAmount <= 0m).Sum(row => row.InstallmentAmount)
            },
            new GetSalesDashboardOverviewOperation.DashboardInstallmentStatusSummaryResponse
            {
                Status = "due_soon",
                Count = installmentRows.Count(row => row.RemainingAmount > 0m && row.DueDate.Date >= today && row.DueDate.Date < dueSoonLimit.Date),
                Amount = installmentRows
                    .Where(row => row.RemainingAmount > 0m && row.DueDate.Date >= today && row.DueDate.Date < dueSoonLimit.Date)
                    .Sum(row => row.RemainingAmount)
            },
            new GetSalesDashboardOverviewOperation.DashboardInstallmentStatusSummaryResponse
            {
                Status = "overdue",
                Count = installmentRows.Count(row => row.RemainingAmount > 0m && row.DueDate.Date < today),
                Amount = installmentRows
                    .Where(row => row.RemainingAmount > 0m && row.DueDate.Date < today)
                    .Sum(row => row.RemainingAmount)
            }
        ];
    }
}
