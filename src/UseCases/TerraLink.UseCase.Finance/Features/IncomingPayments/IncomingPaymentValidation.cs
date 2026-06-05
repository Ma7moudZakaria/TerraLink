using ErrorOr;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Enums;
using TerraLink.UseCase.Finance.Features.IncomingPayments.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments;

internal static class IncomingPaymentValidation
{
    public static async Task<Error?> ValidateAsync(
        IRepository<ContractEntity> contracts,
        IRepository<IncomingPaymentEntity> incomingPayments,
        Guid contractId,
        Guid clientId,
        IncomingPaymentSourceType sourceType,
        Guid? contractInstallmentId,
        decimal amount,
        Guid? excludePaymentId,
        CancellationToken ct)
    {
        if (amount <= 0)
            return Error.Validation(ErrorCode.NoItemCorrect, "Payment amount must be greater than zero.");

        ContractEntity? contract = await contracts.GetAsync(new ContractWithInstallmentsSpec(contractId), ct);
        if (contract is null)
            return Error.Validation(ErrorCode.NoItemExist, "Contract is not found.");

        if (contract.ClientId != clientId)
            return Error.Validation(ErrorCode.NoItemCorrect, "Client does not match contract owner.");

        if (sourceType == IncomingPaymentSourceType.Installment)
        {
            if (contractInstallmentId is null || contractInstallmentId == Guid.Empty)
                return Error.Validation(ErrorCode.NoItemCorrect, "Installment source requires ContractInstallmentId.");

            ContractInstallmentEntity? installment = contract.Installments.FirstOrDefault(i => !i.IsDeleted && i.Id == contractInstallmentId);
            if (installment is null)
                return Error.Validation(ErrorCode.NoItemCorrect, "Installment does not belong to contract.");

            IReadOnlyList<IncomingPaymentEntity> installmentPayments =
                await incomingPayments.ListAsync(new IncomingPaymentsForInstallmentSpec(installment.Id, excludePaymentId), ct);

            decimal paidBefore = installmentPayments.Sum(payment => payment.Amount);
            if (paidBefore + amount > installment.Amount)
                return Error.Validation(ErrorCode.NoItemCorrect, "Installment payment exceeds installment amount.");

            return null;
        }

        if (sourceType == IncomingPaymentSourceType.DownPayment)
        {
            if (contractInstallmentId is not null)
                return Error.Validation(ErrorCode.NoItemCorrect, "Down payment source must not include ContractInstallmentId.");

            decimal installmentsTotal = contract.Installments.Where(i => !i.IsDeleted).Sum(i => i.Amount);
            decimal downPaymentTarget = contract.TotalPrice - installmentsTotal;
            if (downPaymentTarget < 0)
                return Error.Validation(ErrorCode.NoItemCorrect, "Contract installments exceed total contract price.");

            IReadOnlyList<IncomingPaymentEntity> downPayments =
                await incomingPayments.ListAsync(new IncomingDownPaymentsForContractSpec(contract.Id, clientId, excludePaymentId), ct);

            decimal paidBefore = downPayments.Sum(payment => payment.Amount);
            if (paidBefore + amount > downPaymentTarget)
                return Error.Validation(ErrorCode.NoItemCorrect, "Down payment exceeds contract down payment amount.");

            return null;
        }

        return Error.Validation(ErrorCode.NoItemCorrect, "Invalid incoming payment source type.");
    }
}
