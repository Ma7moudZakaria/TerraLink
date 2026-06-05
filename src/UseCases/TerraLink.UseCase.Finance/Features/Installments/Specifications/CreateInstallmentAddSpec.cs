using LowCodeHub.QueryableExtensions.Specifications;
using TerraLink.Domain.Entities;

namespace TerraLink.UseCase.Finance.Features.Installments.Specifications;

public sealed class CreateInstallmentAddSpec(
    Guid contractId,
    string description,
    DateTime dueDate,
    decimal amount,
    string amountText) : IAddSpecification<ContractInstallmentEntity>
{
    public ContractInstallmentEntity Add() => new()
    {
        Id = Guid.CreateVersion7(),
        ContractId = contractId,
        Description = description,
        DueDate = dueDate,
        Amount = amount,
        AmountText = amountText,
        IsDeleted = false
    };
}
