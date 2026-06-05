using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Enums;
using TerraLink.UseCase.Dashboard.Features.Overview.Specifications;

namespace TerraLink.UseCase.Dashboard.Features.Overview.Operations;

public sealed partial class GetDashboardOverviewOperation(
    IRepository<ContractEntity> contracts,
    IRepository<IncomingPaymentEntity> incomingPayments,
    IRepository<ClientEntity> clients,
    IRepository<ContractInstallmentEntity> installments)
    : IOperationHandler<GetDashboardOverviewOperation.Request, GetDashboardOverviewOperation.Response>
{
    public async Task<ErrorOr<GetDashboardOverviewOperation.Response>> HandleAsync(GetDashboardOverviewOperation.Request request, CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime monthStart = new(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime monthEnd = monthStart.AddMonths(1);

        IReadOnlyList<ContractEntity> activeContracts = await contracts.ListAsync(new ActiveDashboardContractsSpec(), ct);
        IReadOnlyList<IncomingPaymentEntity> activePayments = await incomingPayments.ListAsync(new ActiveDashboardIncomingPaymentsSpec(), ct);
        IReadOnlyList<ClientEntity> activeClients = await clients.ListAsync(new ActiveDashboardClientsSpec(), ct);

        decimal totalInstallments = (await installments.ListAsync(new ActiveDashboardInstallmentsWithContractClientSpec(), ct))
            .Sum(installment => installment.Amount);

        decimal totalPaidInstallments = activePayments
            .Where(payment => payment.SourceType == IncomingPaymentSourceType.Installment)
            .Sum(payment => payment.Amount);

        decimal collectionRate = totalInstallments <= 0m
            ? 0m
            : Math.Round((totalPaidInstallments / totalInstallments) * 100m, 2);

        return new GetDashboardOverviewOperation.Response(
            activeContracts.Count,
            activeContracts.Sum(contract => contract.TotalPrice),
            activePayments
                .Where(payment => payment.PaymentDate >= monthStart && payment.PaymentDate < monthEnd)
                .Sum(payment => payment.Amount),
            0m,
            activeClients.Count(client => client.CreatedDate >= monthStart && client.CreatedDate < monthEnd),
            await GetDueInstallmentsThisMonthAsync(monthStart, monthEnd, ct),
            collectionRate,
            activePayments
                .Where(payment => payment.PaymentDate >= monthStart && payment.PaymentDate < monthEnd)
                .Sum(payment => payment.Amount));
    }

    private async Task<decimal> GetDueInstallmentsThisMonthAsync(DateTime monthStart, DateTime monthEnd, CancellationToken ct)
    {
        IReadOnlyList<ContractInstallmentEntity> installmentEntities = await installments.ListAsync(
            new ActiveDashboardInstallmentsWithContractClientSpec(),
            ct);

        Guid[] installmentIds = installmentEntities.Select(installment => installment.Id).ToArray();
        IReadOnlyList<IncomingPaymentEntity> paymentEntities = installmentIds.Length == 0
            ? []
            : await incomingPayments.ListAsync(new ActiveDashboardInstallmentPaymentsSpec(installmentIds), ct);

        Dictionary<Guid, decimal> paidByInstallment = paymentEntities
            .Where(payment => payment.ContractInstallmentId.HasValue)
            .GroupBy(payment => payment.ContractInstallmentId!.Value)
            .ToDictionary(group => group.Key, group => group.Sum(payment => payment.Amount));

        return installmentEntities
            .Where(installment => installment.DueDate >= monthStart && installment.DueDate < monthEnd)
            .Sum(installment =>
            {
                decimal paid = paidByInstallment.TryGetValue(installment.Id, out decimal value) ? value : 0m;
                return Math.Max(0m, installment.Amount - paid);
            });
    }
}
