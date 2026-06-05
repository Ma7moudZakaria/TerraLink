using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.Installments.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Finance.Features.Installments.Operations;

internal static class InstallmentValidation
{
    public static async Task<Error?> ValidateAsync(
        IRepository<ContractEntity> contracts,
        Guid contractId,
        string description,
        DateTime dueDate,
        decimal amount,
        string amountText,
        Guid? installmentId,
        CancellationToken ct)
    {
        if (contractId == Guid.Empty)
            return Error.Validation(ErrorCode.NotValidId, "ContractId is required.");
        if (string.IsNullOrWhiteSpace(description))
            return Error.Validation(ErrorCode.NoItemCorrect, "Description is required.");
        if (dueDate == default)
            return Error.Validation(ErrorCode.NoItemCorrect, "DueDate is required.");
        if (amount <= 0)
            return Error.Validation(ErrorCode.NoItemCorrect, "Amount must be greater than zero.");
        if (string.IsNullOrWhiteSpace(amountText))
            return Error.Validation(ErrorCode.NoItemCorrect, "AmountText is required.");

        ContractEntity? contract = await contracts.GetAsync(new InstallmentContractWithInstallmentsSpec(contractId), ct);
        if (contract is null)
            return Error.Validation(ErrorCode.NoItemExist, "Contract is not found.");

        decimal otherInstallmentsTotal = contract.Installments
            .Where(a => !a.IsDeleted && a.Id != installmentId)
            .Sum(a => a.Amount);

        if (otherInstallmentsTotal + amount > contract.TotalPrice)
            return Error.Validation(ErrorCode.NoItemCorrect, "Installments total exceeds contract total price.");

        return null;
    }
}
