using LowCodeHub.QueryableExtensions.Specifications;
using LowCodeHub.QueryableExtensions.ValueObjects;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Enums;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Specifications;

public sealed class CreateIncomingPaymentAddSpec(
    Guid id,
    string code,
    Guid contractId,
    Guid? contractInstallmentId,
    Guid clientId,
    IncomingPaymentSourceType sourceType,
    Guid transactionTypeId,
    decimal amount,
    Guid paymentMethodId,
    DateTime paymentDate,
    string notes,
    AttributesDictionary? attachments) : IAddSpecification<IncomingPaymentEntity>
{
    public IncomingPaymentEntity Add() => new()
    {
        Id = id,
        Code = code,
        ContractId = contractId,
        ContractInstallmentId = contractInstallmentId,
        ClientId = clientId,
        SourceType = sourceType,
        TransactionTypeId = transactionTypeId,
        Amount = amount,
        PaymentMethodId = paymentMethodId,
        PaymentDate = paymentDate,
        Notes = notes,
        Attachments = attachments
    };
}
