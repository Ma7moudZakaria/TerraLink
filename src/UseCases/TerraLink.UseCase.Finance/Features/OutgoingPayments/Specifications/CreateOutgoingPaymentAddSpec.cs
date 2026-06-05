using LowCodeHub.QueryableExtensions.Specifications;
using LowCodeHub.QueryableExtensions.ValueObjects;
using TerraLink.Domain.Entities;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Specifications;

public sealed class CreateOutgoingPaymentAddSpec(
    Guid id,
    string code,
    Guid expenseTypeId,
    Guid beneficiaryId,
    Guid? unitId,
    Guid? buildingId,
    decimal amount,
    Guid paymentMethodId,
    DateTime paymentDate,
    string notes,
    AttributesDictionary? attachments) : IAddSpecification<OutgoingPaymentEntity>
{
    public OutgoingPaymentEntity Add() => new()
    {
        Id = id,
        Code = code,
        ExpenseTypeId = expenseTypeId,
        BeneficiaryId = beneficiaryId,
        UnitId = unitId,
        BuildingId = buildingId,
        Amount = amount,
        PaymentMethodId = paymentMethodId,
        PaymentDate = paymentDate,
        Notes = notes,
        Attachments = attachments
    };
}
