using LowCodeHub.QueryableExtensions.Specifications;
using Microsoft.EntityFrameworkCore;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Enums;

namespace TerraLink.UseCase.Sales.Features.Shared.Specifications;

public sealed class ActiveSalesContractsOverviewSpec : ISpecification<ContractEntity>
{
    public IQueryable<ContractEntity> Where(IQueryable<ContractEntity> query)
        => query.AsNoTracking().Where(entity => !entity.IsDeleted);
}

public sealed class ActiveSalesClientsOverviewSpec : ISpecification<ClientEntity>
{
    public IQueryable<ClientEntity> Where(IQueryable<ClientEntity> query)
        => query.AsNoTracking().Where(entity => !entity.IsDeleted);
}

public sealed class ActiveSalesIncomingPaymentsOverviewSpec : ISpecification<IncomingPaymentEntity>
{
    public IQueryable<IncomingPaymentEntity> Where(IQueryable<IncomingPaymentEntity> query)
        => query.AsNoTracking().Where(entity => !entity.IsDeleted);
}

public sealed class ActiveSalesInstallmentsWithContractClientOverviewSpec : ISpecification<ContractInstallmentEntity>
{
    public IQueryable<ContractInstallmentEntity> Where(IQueryable<ContractInstallmentEntity> query)
        => query.AsNoTracking()
            .Include(entity => entity.Contract)
                .ThenInclude(contract => contract.Client)
            .Where(entity => !entity.IsDeleted
                && !entity.Contract.IsDeleted
                && !entity.Contract.Client.IsDeleted);
}

public sealed class ActiveSalesInstallmentPaymentsOverviewSpec(IReadOnlyCollection<Guid> installmentIds)
    : ISpecification<IncomingPaymentEntity>
{
    public IQueryable<IncomingPaymentEntity> Where(IQueryable<IncomingPaymentEntity> query)
        => query.AsNoTracking()
            .Where(entity => !entity.IsDeleted
                && entity.SourceType == IncomingPaymentSourceType.Installment
                && entity.ContractInstallmentId != null
                && installmentIds.Contains(entity.ContractInstallmentId.Value));
}
