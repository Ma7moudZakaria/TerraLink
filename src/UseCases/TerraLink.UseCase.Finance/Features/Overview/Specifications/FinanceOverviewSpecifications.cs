using LowCodeHub.QueryableExtensions.Specifications;
using Microsoft.EntityFrameworkCore;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Enums;

namespace TerraLink.UseCase.Finance.Features.Overview.Specifications;

public sealed class ActiveFinanceIncomingPaymentsSpec : ISpecification<IncomingPaymentEntity>
{
    public IQueryable<IncomingPaymentEntity> Where(IQueryable<IncomingPaymentEntity> query)
        => query.AsNoTracking().Where(entity => !entity.IsDeleted);
}

public sealed class ActiveFinanceInstallmentsWithContractClientSpec : ISpecification<ContractInstallmentEntity>
{
    public IQueryable<ContractInstallmentEntity> Where(IQueryable<ContractInstallmentEntity> query)
        => query.AsNoTracking()
            .Include(entity => entity.Contract)
                .ThenInclude(contract => contract.Client)
            .Where(entity => !entity.IsDeleted
                && !entity.Contract.IsDeleted
                && !entity.Contract.Client.IsDeleted);
}

public sealed class ActiveFinanceInstallmentPaymentsSpec(IReadOnlyCollection<Guid> installmentIds)
    : ISpecification<IncomingPaymentEntity>
{
    public IQueryable<IncomingPaymentEntity> Where(IQueryable<IncomingPaymentEntity> query)
        => query.AsNoTracking()
            .Where(entity => !entity.IsDeleted
                && entity.SourceType == IncomingPaymentSourceType.Installment
                && entity.ContractInstallmentId != null
                && installmentIds.Contains(entity.ContractInstallmentId.Value));
}
