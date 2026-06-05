using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Shared.Specifications;

namespace TerraLink.UseCase.Sales.Features.Shared.Operations;

internal sealed record SalesInstallmentStatusRow(
    Guid InstallmentId,
    Guid ContractId,
    string ContractNumber,
    Guid ClientId,
    string ClientName,
    DateTime DueDate,
    decimal InstallmentAmount,
    decimal PaidAmount,
    decimal RemainingAmount);

internal static class SalesOverviewReadModels
{
    public static async Task<List<SalesInstallmentStatusRow>> GetInstallmentStatusRowsAsync(
        IRepository<ContractInstallmentEntity> installments,
        IRepository<IncomingPaymentEntity> incomingPayments,
        CancellationToken ct)
    {
        IReadOnlyList<ContractInstallmentEntity> installmentEntities = await installments.ListAsync(
            new ActiveSalesInstallmentsWithContractClientOverviewSpec(),
            ct);

        Guid[] installmentIds = installmentEntities.Select(entity => entity.Id).ToArray();

        IReadOnlyList<IncomingPaymentEntity> paymentEntities = installmentIds.Length == 0
            ? []
            : await incomingPayments.ListAsync(new ActiveSalesInstallmentPaymentsOverviewSpec(installmentIds), ct);

        Dictionary<Guid, decimal> paidByInstallment = paymentEntities
            .Where(payment => payment.ContractInstallmentId.HasValue)
            .GroupBy(payment => payment.ContractInstallmentId!.Value)
            .ToDictionary(group => group.Key, group => group.Sum(payment => payment.Amount));

        return installmentEntities
            .Select(installment =>
            {
                decimal paidAmount = paidByInstallment.TryGetValue(installment.Id, out decimal value) ? value : 0m;
                decimal remaining = Math.Max(0m, installment.Amount - paidAmount);

                return new SalesInstallmentStatusRow(
                    installment.Id,
                    installment.ContractId,
                    installment.Contract.ContractNumber,
                    installment.Contract.ClientId,
                    installment.Contract.Client.Name,
                    installment.DueDate,
                    installment.Amount,
                    paidAmount,
                    remaining);
            })
            .ToList();
    }
}
