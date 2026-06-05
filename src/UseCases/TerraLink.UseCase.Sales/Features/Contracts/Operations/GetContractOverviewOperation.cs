using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Shared.Operations;
using TerraLink.UseCase.Sales.Features.Shared.Specifications;
using TerraLink.UseCase.Sales.Features.Contracts.Endpoints;

namespace TerraLink.UseCase.Sales.Features.Contracts.Operations;

public sealed partial class GetContractOverviewOperation(
    IRepository<ContractEntity> contracts,
    IRepository<ContractInstallmentEntity> installments,
    IRepository<IncomingPaymentEntity> incomingPayments)
    : IOperationHandler<GetContractOverviewOperation.Request, GetContractOverviewOperation.Response>
{
    public async Task<ErrorOr<GetContractOverviewOperation.Response>> HandleAsync(GetContractOverviewOperation.Request request, CancellationToken ct = default)
    {
        DateTime utcNow = DateTime.UtcNow;
        DateTime monthStart = new(utcNow.Year, utcNow.Month, 1);
        DateTime monthEnd = monthStart.AddMonths(1);
        DateTime prevMonthStart = monthStart.AddMonths(-1);
        DateTime yearStart = new(utcNow.Year, 1, 1);
        DateTime yearEnd = yearStart.AddYears(1);
        DateTime today = utcNow.Date;
        DateTime dueSoonLimit = today.AddDays(7);

        IReadOnlyList<ContractEntity> activeContracts = await contracts.ListAsync(new ActiveSalesContractsOverviewSpec(), ct);

        decimal monthlySales = activeContracts
            .Where(contract => contract.ContractDate >= monthStart && contract.ContractDate < monthEnd)
            .Sum(contract => contract.TotalPrice);

        decimal previousMonthSales = activeContracts
            .Where(contract => contract.ContractDate >= prevMonthStart && contract.ContractDate < monthStart)
            .Sum(contract => contract.TotalPrice);

        List<SalesInstallmentStatusRow> unpaidInstallmentRows = (await SalesOverviewReadModels.GetInstallmentStatusRowsAsync(installments, incomingPayments, ct))
            .Where(row => row.RemainingAmount > 0m)
            .ToList();

        List<GetContractOverviewOperation.MonthlySalesPointResponse> monthlySalesSeries = activeContracts
            .Where(contract => contract.ContractDate >= yearStart && contract.ContractDate < yearEnd)
            .GroupBy(contract => contract.ContractDate.Month)
            .Select(group => new GetContractOverviewOperation.MonthlySalesPointResponse
            {
                Month = group.Key,
                Value = group.Sum(contract => contract.TotalPrice)
            })
            .OrderBy(point => point.Month)
            .ToList();

        List<GetContractOverviewOperation.ContractUrgentActionResponse> urgentActions = unpaidInstallmentRows
            .Where(row => row.DueDate < dueSoonLimit)
            .OrderBy(row => row.DueDate)
            .Take(10)
            .Select(row => new GetContractOverviewOperation.ContractUrgentActionResponse
            {
                ContractId = row.ContractId,
                ContractNumber = row.ContractNumber,
                ClientName = row.ClientName,
                DueDate = row.DueDate,
                RemainingAmount = row.RemainingAmount,
                Status = row.DueDate < today ? "overdue" : "due_soon"
            })
            .ToList();

        return new GetContractOverviewOperation.Response
        {
            TotalContracts = activeContracts.Count,
            ContractsThisMonth = activeContracts.Count(contract => contract.ContractDate >= monthStart && contract.ContractDate < monthEnd),
            MonthlySalesValue = monthlySales,
            AnnualSalesValue = activeContracts
                .Where(contract => contract.ContractDate >= yearStart && contract.ContractDate < yearEnd)
                .Sum(contract => contract.TotalPrice),
            GrowthPercentage = previousMonthSales <= 0m
                ? monthlySales > 0m ? 100m : 0m
                : Math.Round(((monthlySales - previousMonthSales) / previousMonthSales) * 100m, 2),
            DueInstallmentsThisMonth = unpaidInstallmentRows
                .Where(row => row.DueDate >= monthStart && row.DueDate < monthEnd)
                .Sum(row => row.RemainingAmount),
            OverdueInstallmentsAmount = unpaidInstallmentRows
                .Where(row => row.DueDate < today)
                .Sum(row => row.RemainingAmount),
            MonthlySales = monthlySalesSeries,
            UrgentActions = urgentActions
        };
    }
}
