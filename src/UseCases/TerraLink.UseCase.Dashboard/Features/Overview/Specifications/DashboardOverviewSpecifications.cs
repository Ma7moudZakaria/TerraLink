using LowCodeHub.QueryableExtensions.Specifications;
using Microsoft.EntityFrameworkCore;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Enums;

namespace TerraLink.UseCase.Dashboard.Features.Overview.Specifications;

public sealed class ActiveDashboardContractsSpec : ISpecification<ContractEntity>
{
    public IQueryable<ContractEntity> Where(IQueryable<ContractEntity> query)
        => query.AsNoTracking().Where(entity => !entity.IsDeleted);
}

public sealed class ActiveDashboardClientsSpec : ISpecification<ClientEntity>
{
    public IQueryable<ClientEntity> Where(IQueryable<ClientEntity> query)
        => query.AsNoTracking().Where(entity => !entity.IsDeleted);
}

public sealed class ActiveDashboardIncomingPaymentsSpec : ISpecification<IncomingPaymentEntity>
{
    public IQueryable<IncomingPaymentEntity> Where(IQueryable<IncomingPaymentEntity> query)
        => query.AsNoTracking().Where(entity => !entity.IsDeleted);
}

public sealed class ActiveDashboardInstallmentsWithContractClientSpec : ISpecification<ContractInstallmentEntity>
{
    public IQueryable<ContractInstallmentEntity> Where(IQueryable<ContractInstallmentEntity> query)
        => query.AsNoTracking()
            .Include(entity => entity.Contract)
                .ThenInclude(contract => contract.Client)
            .Where(entity => !entity.IsDeleted
                && !entity.Contract.IsDeleted
                && !entity.Contract.Client.IsDeleted);
}

public sealed class ActiveDashboardInstallmentPaymentsSpec(IReadOnlyCollection<Guid> installmentIds)
    : ISpecification<IncomingPaymentEntity>
{
    public IQueryable<IncomingPaymentEntity> Where(IQueryable<IncomingPaymentEntity> query)
        => query.AsNoTracking()
            .Where(entity => !entity.IsDeleted
                && entity.SourceType == IncomingPaymentSourceType.Installment
                && entity.ContractInstallmentId != null
                && installmentIds.Contains(entity.ContractInstallmentId.Value));
}

public sealed class DashboardUserRolesWithRoleSpec : ISpecification<UserRoleEntity>
{
    public IQueryable<UserRoleEntity> Where(IQueryable<UserRoleEntity> query)
        => query.AsNoTracking().Include(entity => entity.Role);
}
