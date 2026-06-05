using LowCodeHub.QueryableExtensions.Specifications;
using LowCodeHub.QueryableExtensions.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Enums;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Specifications;

public sealed class IncomingPaymentDetailsSpec(Guid id) : ISpecification<IncomingPaymentEntity>
{
    public IQueryable<IncomingPaymentEntity> Where(IQueryable<IncomingPaymentEntity> query)
    {
        return IncludeReadModel(query)
            .Where(entity => entity.Id == id && !entity.IsDeleted);
    }

    internal static IQueryable<IncomingPaymentEntity> IncludeReadModel(IQueryable<IncomingPaymentEntity> query)
    {
        return query
            .Include(entity => entity.Client)
            .Include(entity => entity.Contract)
                .ThenInclude(contract => contract.Unit)
            .Include(entity => entity.TransactionType)
            .Include(entity => entity.PaymentMethod);
    }
}

public sealed class IncomingPaymentsListSpec(
    string? code,
    string? contractCode,
    string? clientName,
    string? unitCode,
    decimal? amount,
    DateTime? paymentDate) : ISpecification<IncomingPaymentEntity>
{
    public IQueryable<IncomingPaymentEntity> Where(IQueryable<IncomingPaymentEntity> query)
    {
        query = IncomingPaymentDetailsSpec.IncludeReadModel(query)
            .Where(entity => !entity.IsDeleted);

        if (!string.IsNullOrWhiteSpace(code))
        {
            query = query.Where(entity => entity.Code.Contains(code));
        }

        if (!string.IsNullOrWhiteSpace(contractCode))
        {
            query = query.Where(entity => entity.Contract.ContractNumber.Contains(contractCode));
        }

        if (!string.IsNullOrWhiteSpace(clientName))
        {
            query = query.Where(entity => entity.Client.Name.Contains(clientName));
        }

        if (!string.IsNullOrWhiteSpace(unitCode))
        {
            query = query.Where(entity => entity.Contract.Unit != null && entity.Contract.Unit.Number.Contains(unitCode));
        }

        if (amount.HasValue)
        {
            query = query.Where(entity => entity.Amount == amount.Value);
        }

        if (paymentDate.HasValue)
        {
            query = query.Where(entity => entity.PaymentDate.Date == paymentDate.Value.Date);
        }

        return query.OrderByDescending(entity => entity.PaymentDate).ThenBy(entity => entity.Code);
    }
}

public sealed class SoftDeleteIncomingPaymentUpdateSpec : IUpdateSpecification<IncomingPaymentEntity>
{
    public Action<UpdateSettersBuilder<IncomingPaymentEntity>> Update()
    {
        return setters => setters.SetProperty(entity => entity.IsDeleted, true);
    }
}

public sealed class IncomingPaymentByIdSpec(Guid id) : ISpecification<IncomingPaymentEntity>
{
    public IQueryable<IncomingPaymentEntity> Where(IQueryable<IncomingPaymentEntity> query)
    {
        return query.Where(entity => entity.Id == id && !entity.IsDeleted);
    }
}

public sealed class UpdateIncomingPaymentFieldsSpec : IUpdateSpecification<IncomingPaymentEntity>
{
    public required Guid ContractId { get; init; }
    public required Guid ClientId { get; init; }
    public Guid? ContractInstallmentId { get; init; }
    public IncomingPaymentSourceType SourceType { get; init; }
    public required Guid TransactionTypeId { get; init; }
    public required decimal Amount { get; init; }
    public required Guid PaymentMethodId { get; init; }
    public DateTime PaymentDate { get; init; }
    public string Notes { get; init; } = string.Empty;
    public AttributesDictionary? Attachments { get; init; }

    public Action<UpdateSettersBuilder<IncomingPaymentEntity>> Update()
    {
        return setters => setters
            .SetProperty(entity => entity.ClientId, ClientId)
            .SetProperty(entity => entity.SourceType, SourceType)
            .SetProperty(entity => entity.TransactionTypeId, TransactionTypeId)
            .SetProperty(entity => entity.Amount, Amount)
            .SetProperty(entity => entity.PaymentMethodId, PaymentMethodId)
            .SetProperty(entity => entity.PaymentDate, PaymentDate)
            .SetProperty(entity => entity.Notes, Notes)
            .SetProperty(entity => entity.ContractId, ContractId)
            .SetProperty(entity => entity.ContractInstallmentId, ContractInstallmentId)
            .SetProperty(entity => entity.Attachments, Attachments);
    }
}

public sealed class IncomingPaymentsForInstallmentSpec(Guid installmentId, Guid? excludePaymentId)
    : ISpecification<IncomingPaymentEntity>
{
    public IQueryable<IncomingPaymentEntity> Where(IQueryable<IncomingPaymentEntity> query)
    {
        query = query.Where(entity => !entity.IsDeleted
            && entity.SourceType == IncomingPaymentSourceType.Installment
            && entity.ContractInstallmentId == installmentId);

        if (excludePaymentId.HasValue)
        {
            query = query.Where(entity => entity.Id != excludePaymentId.Value);
        }

        return query;
    }
}

public sealed class IncomingDownPaymentsForContractSpec(Guid contractId, Guid clientId, Guid? excludePaymentId)
    : ISpecification<IncomingPaymentEntity>
{
    public IQueryable<IncomingPaymentEntity> Where(IQueryable<IncomingPaymentEntity> query)
    {
        query = query.Where(entity => !entity.IsDeleted
            && entity.SourceType == IncomingPaymentSourceType.DownPayment
            && entity.ContractId == contractId
            && entity.ClientId == clientId);

        if (excludePaymentId.HasValue)
        {
            query = query.Where(entity => entity.Id != excludePaymentId.Value);
        }

        return query;
    }
}

public sealed class ContractWithInstallmentsSpec(Guid contractId) : ISpecification<ContractEntity>
{
    public IQueryable<ContractEntity> Where(IQueryable<ContractEntity> query)
    {
        return query
            .Include(entity => entity.Installments)
            .Where(entity => entity.Id == contractId && !entity.IsDeleted);
    }
}

public sealed class InstallmentsPaymentStatusByContractSpec(Guid contractId) : ISpecification<ContractInstallmentEntity>
{
    public IQueryable<ContractInstallmentEntity> Where(IQueryable<ContractInstallmentEntity> query)
    {
        return query
            .Include(entity => entity.IncomingPayments)
            .Where(entity => entity.ContractId == contractId && !entity.IsDeleted)
            .OrderBy(entity => entity.DueDate);
    }
}
