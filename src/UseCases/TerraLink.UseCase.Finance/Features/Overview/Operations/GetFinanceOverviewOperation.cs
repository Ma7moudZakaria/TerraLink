using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Enums;
using TerraLink.UseCase.Finance.Features.Overview.Specifications;

namespace TerraLink.UseCase.Finance.Features.Overview.Operations;

public sealed partial class GetFinanceOverviewOperation(
    IRepository<IncomingPaymentEntity> incomingPayments,
    IRepository<ContractInstallmentEntity> installments)
    : IOperationHandler<GetFinanceOverviewOperation.Request, GetFinanceOverviewOperation.Response>
{
    public async Task<ErrorOr<GetFinanceOverviewOperation.Response>> HandleAsync(GetFinanceOverviewOperation.Request request, CancellationToken ct = default)
    {
        DateTime utcNow = DateTime.UtcNow;
        DateTime monthStart = new(utcNow.Year, utcNow.Month, 1);
        DateTime monthEnd = monthStart.AddMonths(1);
        DateTime yearStart = new(utcNow.Year, 1, 1);
        DateTime yearEnd = yearStart.AddYears(1);
        DateTime today = utcNow.Date;
        DateTime dueSoonLimit = today.AddDays(7);

        IReadOnlyList<IncomingPaymentEntity> activePayments = await incomingPayments.ListAsync(new ActiveFinanceIncomingPaymentsSpec(), ct);
        List<InstallmentStatusRow> installmentRows = await GetInstallmentStatusRowsAsync(ct);

        decimal totalInstallments = installmentRows.Sum(row => row.InstallmentAmount);
        decimal totalPaidInstallments = activePayments
            .Where(payment => payment.SourceType == IncomingPaymentSourceType.Installment)
            .Sum(payment => payment.Amount);

        decimal collectionRate = totalInstallments <= 0m
            ? 0m
            : Math.Round((totalPaidInstallments / totalInstallments) * 100m, 2);

        List<GetFinanceOverviewOperation.MonthlyInstallmentsPointResponse> monthlySeries = Enumerable.Range(1, 12)
            .Select(month => new GetFinanceOverviewOperation.MonthlyInstallmentsPointResponse(
                month,
                installmentRows
                    .Where(row => row.DueDate >= yearStart && row.DueDate < yearEnd && row.DueDate.Month == month)
                    .Sum(row => row.InstallmentAmount),
                activePayments
                    .Where(payment => payment.SourceType == IncomingPaymentSourceType.Installment
                        && payment.PaymentDate >= yearStart
                        && payment.PaymentDate < yearEnd
                        && payment.PaymentDate.Month == month)
                    .Sum(payment => payment.Amount)))
            .ToList();

        List<InstallmentStatusRow> unpaidRows = installmentRows
            .Where(row => row.RemainingAmount > 0m)
            .ToList();

        List<GetFinanceOverviewOperation.UrgentActionResponse> urgentActions = unpaidRows
            .Where(row => row.DueDate < dueSoonLimit)
            .OrderBy(row => row.DueDate)
            .Take(10)
            .Select(row => new GetFinanceOverviewOperation.UrgentActionResponse(
                row.ContractId,
                row.ContractNumber,
                row.InstallmentId,
                row.ClientName,
                row.DueDate,
                row.RemainingAmount,
                row.DueDate < today ? "overdue" : "due_soon"))
            .ToList();

        return new GetFinanceOverviewOperation.Response(
            activePayments
                .Where(payment => payment.PaymentDate >= monthStart && payment.PaymentDate < monthEnd)
                .Sum(payment => payment.Amount),
            activePayments
                .Where(payment => payment.PaymentDate >= yearStart && payment.PaymentDate < yearEnd)
                .Sum(payment => payment.Amount),
            unpaidRows
                .Where(row => row.DueDate >= monthStart && row.DueDate < monthEnd)
                .Sum(row => row.RemainingAmount),
            activePayments
                .Where(payment => payment.SourceType == IncomingPaymentSourceType.Installment
                    && payment.PaymentDate >= monthStart
                    && payment.PaymentDate < monthEnd)
                .Sum(payment => payment.Amount),
            unpaidRows
                .Where(row => row.DueDate < today)
                .Sum(row => row.RemainingAmount),
            collectionRate,
            monthlySeries,
            urgentActions);
    }

    private async Task<List<InstallmentStatusRow>> GetInstallmentStatusRowsAsync(CancellationToken ct)
    {
        IReadOnlyList<ContractInstallmentEntity> installmentEntities = await installments.ListAsync(
            new ActiveFinanceInstallmentsWithContractClientSpec(),
            ct);

        Guid[] installmentIds = installmentEntities.Select(entity => entity.Id).ToArray();

        IReadOnlyList<IncomingPaymentEntity> paymentEntities = installmentIds.Length == 0
            ? []
            : await incomingPayments.ListAsync(new ActiveFinanceInstallmentPaymentsSpec(installmentIds), ct);

        Dictionary<Guid, decimal> paidByInstallment = paymentEntities
            .Where(payment => payment.ContractInstallmentId.HasValue)
            .GroupBy(payment => payment.ContractInstallmentId!.Value)
            .ToDictionary(group => group.Key, group => group.Sum(payment => payment.Amount));

        return installmentEntities
            .Select(installment =>
            {
                decimal paid = paidByInstallment.TryGetValue(installment.Id, out decimal value) ? value : 0m;
                return new InstallmentStatusRow(
                    installment.Id,
                    installment.ContractId,
                    installment.Contract.ContractNumber,
                    installment.Contract.Client.Name,
                    installment.DueDate,
                    installment.Amount,
                    Math.Max(0m, installment.Amount - paid));
            })
            .ToList();
    }

    private sealed record InstallmentStatusRow(
        Guid InstallmentId,
        Guid ContractId,
        string ContractNumber,
        string ClientName,
        DateTime DueDate,
        decimal InstallmentAmount,
        decimal RemainingAmount);
}
