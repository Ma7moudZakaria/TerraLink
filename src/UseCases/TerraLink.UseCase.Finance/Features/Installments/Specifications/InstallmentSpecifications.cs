using LowCodeHub.QueryableExtensions.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TerraLink.Domain.Entities;

namespace TerraLink.UseCase.Finance.Features.Installments.Specifications;

public sealed class InstallmentContractWithInstallmentsSpec(Guid contractId) : ISpecification<ContractEntity>
{
    public IQueryable<ContractEntity> Where(IQueryable<ContractEntity> query)
    {
        return query
            .Include(entity => entity.Installments)
            .Where(entity => entity.Id == contractId && !entity.IsDeleted);
    }
}

public sealed class InstallmentByIdSpec(Guid id) : ISpecification<ContractInstallmentEntity>
{
    public IQueryable<ContractInstallmentEntity> Where(IQueryable<ContractInstallmentEntity> query)
    {
        return IncludeReadModel(query)
            .Where(entity => entity.Id == id && !entity.IsDeleted);
    }

    internal static IQueryable<ContractInstallmentEntity> IncludeReadModel(IQueryable<ContractInstallmentEntity> query)
    {
        return query
            .Include(entity => entity.Contract)
                .ThenInclude(contract => contract.Client)
            .Include(entity => entity.Contract)
                .ThenInclude(contract => contract.Unit)
            .Include(entity => entity.IncomingPayments);
    }
}

public sealed class InstallmentsListSpec(Guid? contractId, string? clientName, string? unit)
    : ISpecification<ContractInstallmentEntity>
{
    public IQueryable<ContractInstallmentEntity> Where(IQueryable<ContractInstallmentEntity> query)
    {
        query = InstallmentByIdSpec.IncludeReadModel(query)
            .Where(entity => !entity.IsDeleted);

        if (contractId.HasValue)
        {
            query = query.Where(entity => entity.ContractId == contractId.Value);
        }

        if (!string.IsNullOrWhiteSpace(clientName))
        {
            query = query.Where(entity => entity.Contract.Client.Name.Contains(clientName));
        }

        if (!string.IsNullOrWhiteSpace(unit))
        {
            query = query.Where(entity => entity.Contract.Unit != null &&
                ((entity.Contract.Unit.Number != null && entity.Contract.Unit.Number.Contains(unit)) ||
                 (entity.Contract.Unit.Name != null && entity.Contract.Unit.Name.Contains(unit))));
        }

        return query.OrderBy(entity => entity.DueDate).ThenBy(entity => entity.CreatedDate);
    }
}

public sealed class UpdateInstallmentFieldsSpec : IUpdateSpecification<ContractInstallmentEntity>
{
    public required Guid ContractId { get; init; }
    public required string Description { get; init; }
    public DateTime DueDate { get; init; }
    public decimal Amount { get; init; }
    public string AmountText { get; init; } = string.Empty;

    public Action<UpdateSettersBuilder<ContractInstallmentEntity>> Update()
    {
        return setters => setters
            .SetProperty(entity => entity.ContractId, ContractId)
            .SetProperty(entity => entity.Description, Description)
            .SetProperty(entity => entity.DueDate, DueDate)
            .SetProperty(entity => entity.Amount, Amount)
            .SetProperty(entity => entity.AmountText, AmountText);
    }
}
