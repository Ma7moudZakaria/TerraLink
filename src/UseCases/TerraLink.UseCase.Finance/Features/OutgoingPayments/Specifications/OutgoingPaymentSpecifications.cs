using LowCodeHub.QueryableExtensions.Specifications;
using LowCodeHub.QueryableExtensions.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TerraLink.Domain.Entities;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Specifications;

public sealed class OutgoingPaymentDetailsSpec(Guid id) : ISpecification<OutgoingPaymentEntity>
{
    public IQueryable<OutgoingPaymentEntity> Where(IQueryable<OutgoingPaymentEntity> query)
    {
        return IncludeReadModel(query)
            .Where(entity => entity.Id == id && !entity.IsDeleted);
    }

    internal static IQueryable<OutgoingPaymentEntity> IncludeReadModel(IQueryable<OutgoingPaymentEntity> query)
    {
        return query
            .Include(entity => entity.ExpenseType)
            .Include(entity => entity.Beneficiary)
            .Include(entity => entity.PaymentMethod)
            .Include(entity => entity.Unit)
            .Include(entity => entity.Building);
    }
}

public sealed class OutgoingPaymentsListSpec(
    string? code,
    string? unitCode,
    string? buildingCode,
    Guid? expenseTypeId,
    Guid? beneficiaryId,
    bool? isUnitRelated,
    decimal? amount,
    DateTime? paymentDate) : ISpecification<OutgoingPaymentEntity>
{
    public IQueryable<OutgoingPaymentEntity> Where(IQueryable<OutgoingPaymentEntity> query)
    {
        query = OutgoingPaymentDetailsSpec.IncludeReadModel(query)
            .Where(entity => !entity.IsDeleted);

        if (!string.IsNullOrWhiteSpace(code))
        {
            query = query.Where(entity => entity.Code.Contains(code));
        }

        if (!string.IsNullOrWhiteSpace(unitCode))
        {
            query = query.Where(entity => entity.Unit != null && entity.Unit.Number.Contains(unitCode));
        }

        if (!string.IsNullOrWhiteSpace(buildingCode))
        {
            query = query.Where(entity => entity.Building != null && entity.Building.Number.Contains(buildingCode));
        }

        if (expenseTypeId.HasValue)
        {
            query = query.Where(entity => entity.ExpenseTypeId == expenseTypeId.Value);
        }

        if (beneficiaryId.HasValue)
        {
            query = query.Where(entity => entity.BeneficiaryId == beneficiaryId.Value);
        }

        if (isUnitRelated.HasValue)
        {
            query = isUnitRelated.Value
                ? query.Where(entity => entity.UnitId != null)
                : query.Where(entity => entity.UnitId == null);
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

public sealed class SoftDeleteOutgoingPaymentUpdateSpec : IUpdateSpecification<OutgoingPaymentEntity>
{
    public Action<UpdateSettersBuilder<OutgoingPaymentEntity>> Update()
    {
        return setters => setters.SetProperty(entity => entity.IsDeleted, true);
    }
}

public sealed class OutgoingPaymentByIdSpec(Guid id) : ISpecification<OutgoingPaymentEntity>
{
    public IQueryable<OutgoingPaymentEntity> Where(IQueryable<OutgoingPaymentEntity> query)
    {
        return query.Where(entity => entity.Id == id && !entity.IsDeleted);
    }
}

public sealed class UpdateOutgoingPaymentFieldsSpec : IUpdateSpecification<OutgoingPaymentEntity>
{
    public required Guid ExpenseTypeId { get; init; }
    public required Guid BeneficiaryId { get; init; }
    public Guid? UnitId { get; init; }
    public Guid? BuildingId { get; init; }
    public required decimal Amount { get; init; }
    public required Guid PaymentMethodId { get; init; }
    public DateTime PaymentDate { get; init; }
    public string Notes { get; init; } = string.Empty;
    public AttributesDictionary? Attachments { get; init; }

    public Action<UpdateSettersBuilder<OutgoingPaymentEntity>> Update()
    {
        return setters => setters
            .SetProperty(entity => entity.ExpenseTypeId, ExpenseTypeId)
            .SetProperty(entity => entity.BeneficiaryId, BeneficiaryId)
            .SetProperty(entity => entity.UnitId, UnitId)
            .SetProperty(entity => entity.BuildingId, BuildingId)
            .SetProperty(entity => entity.Amount, Amount)
            .SetProperty(entity => entity.PaymentMethodId, PaymentMethodId)
            .SetProperty(entity => entity.PaymentDate, PaymentDate)
            .SetProperty(entity => entity.Notes, Notes)
            .SetProperty(entity => entity.Attachments, Attachments);
    }
}
