using LowCodeHub.QueryableExtensions.Specifications;
using LowCodeHub.QueryableExtensions.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Contracts.Endpoints;

namespace TerraLink.UseCase.Sales.Features.Contracts.Specifications;

public sealed class CreateContractAddSpec(ContractEntity contract) : IAddSpecification<ContractEntity>
{
    public ContractEntity Add() => contract;
}

public sealed class ContractByIdSpec(Guid id, bool includeDetails = false) : ISpecification<ContractEntity>
{
    public IQueryable<ContractEntity> Where(IQueryable<ContractEntity> query)
    {
        query = query.Where(contract => contract.Id == id && !contract.IsDeleted);

        if (!includeDetails)
        {
            return query;
        }

        return query.Include(contract => contract.Land)
                    .Include(contract => contract.Unit).ThenInclude(unit => unit!.UnitType)
                    .Include(contract => contract.Building)
                    .Include(contract => contract.PaymentMethod)
                    .Include(contract => contract.User)
                    .Include(contract => contract.ClientContracts).ThenInclude(clientContract => clientContract.Client)
                    .Include(contract => contract.Installments);
    }
}

public sealed class ContractsListSpec(GetAllContractsEndpoint.Request request) : ISpecification<ContractEntity>
{
    public IQueryable<ContractEntity> Where(IQueryable<ContractEntity> query)
    {
        query = query.Where(contract => !contract.IsDeleted)
                     .Include(contract => contract.Land)
                     .Include(contract => contract.Unit)
                     .Include(contract => contract.Building)
                     .Include(contract => contract.PaymentMethod)
                     .Include(contract => contract.ClientContracts).ThenInclude(clientContract => clientContract.Client)
                     .Include(contract => contract.Installments);

        if (!string.IsNullOrWhiteSpace(request.ContractNumber))
        {
            query = query.Where(contract => contract.ContractNumber.Contains(request.ContractNumber));
        }

        if (!string.IsNullOrWhiteSpace(request.ClientName))
        {
            query = query.Where(contract =>
                contract.ClientContracts.Any(clientContract => clientContract.Client.Name.Contains(request.ClientName)));
        }

        if (!string.IsNullOrWhiteSpace(request.UnitNumber))
        {
            query = query.Where(contract => contract.Unit != null && contract.Unit.Number.Contains(request.UnitNumber));
        }

        if (request.ContractTypeId.HasValue)
        {
            query = query.Where(contract => contract.PaymentMethodId == request.ContractTypeId.Value);
        }

        if (request.TotalPrice.HasValue)
        {
            query = query.Where(contract => contract.TotalPrice == request.TotalPrice.Value);
        }

        if (request.UnitPriceAtContract.HasValue)
        {
            query = query.Where(contract => contract.UnitPriceAtContract == request.UnitPriceAtContract.Value);
        }

        if (request.IsInstallmentPlan.HasValue)
        {
            query = query.Where(contract => contract.IsInstallmentPlan == request.IsInstallmentPlan.Value);
        }

        if (request.ContractDate.HasValue)
        {
            query = query.Where(contract => contract.ContractDate.Date == request.ContractDate.Value.Date);
        }

        return query;
    }
}

public sealed class ContractsDropdownSpec(Guid? contractTypeId) : ISpecification<ContractEntity>
{
    public IQueryable<ContractEntity> Where(IQueryable<ContractEntity> query)
    {
        query = query.Where(contract => !contract.IsDeleted);

        if (contractTypeId.HasValue)
        {
            query = query.Where(contract => contract.PaymentMethodId == contractTypeId.Value);
        }

        return query.OrderBy(contract => contract.ContractNumber);
    }
}

public sealed class UpdateContractFieldsSpec(
    UpdateContractEndpoint.Request request,
    Guid clientId,
    Guid userId,
    DateTime now,
    string modifiedBy) : IUpdateSpecification<ContractEntity>
{
    public Action<UpdateSettersBuilder<ContractEntity>> Update()
    {
        return setters => setters
            .SetProperty(contract => contract.ContractDate, request.ContractDate)
            .SetProperty(contract => contract.PaymentMethodId, request.ContractTypeId)
            .SetProperty(contract => contract.TotalPrice, request.TotalPrice)
            .SetProperty(contract => contract.Notes, request.Notes)
            .SetProperty(contract => contract.LandId, request.LandId)
            .SetProperty(contract => contract.BuildingId, request.BuildingId)
            .SetProperty(contract => contract.UnitId, request.UnitId)
            .SetProperty(contract => contract.IsInstallmentPlan, request.IsInstallmentPlan)
            .SetProperty(contract => contract.UnitPriceAtContract, request.UnitPriceAtContract)
            .SetProperty(contract => contract.Attachments, request.Attachments)
            .SetProperty(contract => contract.ClientId, clientId)
            .SetProperty(contract => contract.UserId, userId)
            .SetProperty(contract => contract.UpdatedDate, now)
            .SetProperty(contract => contract.ModifiedBy, modifiedBy);
    }
}

public sealed class SoftDeleteContractSpec(DateTime now, string modifiedBy) : IUpdateSpecification<ContractEntity>
{
    public Action<UpdateSettersBuilder<ContractEntity>> Update()
    {
        return setters => setters
            .SetProperty(contract => contract.IsDeleted, true)
            .SetProperty(contract => contract.UpdatedDate, now)
            .SetProperty(contract => contract.ModifiedBy, modifiedBy);
    }
}

public sealed class ActiveClientContractsSpec(Guid contractId) : ISpecification<ClientContractEntity>
{
    public IQueryable<ClientContractEntity> Where(IQueryable<ClientContractEntity> query)
    {
        return query.Where(clientContract => clientContract.ContractId == contractId && !clientContract.IsDeleted);
    }
}

public sealed class AddClientContractSpec(Guid contractId, Guid clientId, DateTime now, string createdBy) : IAddSpecification<ClientContractEntity>
{
    public ClientContractEntity Add()
    {
        return new ClientContractEntity
        {
            Id = Guid.CreateVersion7(),
            ContractId = contractId,
            ClientId = clientId,
            CreatedBy = createdBy,
            CreatedDate = now
        };
    }
}

public sealed class SoftDeleteClientContractSpec(DateTime now, string modifiedBy) : IUpdateSpecification<ClientContractEntity>
{
    public Action<UpdateSettersBuilder<ClientContractEntity>> Update()
    {
        return setters => setters
            .SetProperty(clientContract => clientContract.IsDeleted, true)
            .SetProperty(clientContract => clientContract.UpdatedDate, now)
            .SetProperty(clientContract => clientContract.ModifiedBy, modifiedBy);
    }
}

public sealed class ActiveContractInstallmentsSpec(Guid contractId) : ISpecification<ContractInstallmentEntity>
{
    public IQueryable<ContractInstallmentEntity> Where(IQueryable<ContractInstallmentEntity> query)
    {
        return query.Where(installment => installment.ContractId == contractId && !installment.IsDeleted);
    }
}

public sealed class ActiveContractInstallmentsByIdsSpec(Guid contractId, IReadOnlyCollection<Guid> installmentIds)
    : ISpecification<ContractInstallmentEntity>
{
    public IQueryable<ContractInstallmentEntity> Where(IQueryable<ContractInstallmentEntity> query)
    {
        return query.Where(installment =>
            installment.ContractId == contractId &&
            !installment.IsDeleted &&
            installmentIds.Contains(installment.Id));
    }
}

public sealed class ActiveInstallmentByIdSpec(Guid installmentId) : ISpecification<ContractInstallmentEntity>
{
    public IQueryable<ContractInstallmentEntity> Where(IQueryable<ContractInstallmentEntity> query)
    {
        return query.Where(installment => installment.Id == installmentId && !installment.IsDeleted);
    }
}

public sealed class SoftDeleteInstallmentSpec(DateTime now, string modifiedBy) : IUpdateSpecification<ContractInstallmentEntity>
{
    public Action<UpdateSettersBuilder<ContractInstallmentEntity>> Update()
    {
        return setters => setters
            .SetProperty(installment => installment.IsDeleted, true)
            .SetProperty(installment => installment.UpdatedDate, now)
            .SetProperty(installment => installment.ModifiedBy, modifiedBy);
    }
}

public sealed class UpdateInstallmentSpec(
    UpdateContractEndpoint.InstallmentItemRequest item,
    DateTime now,
    string modifiedBy) : IUpdateSpecification<ContractInstallmentEntity>
{
    public Action<UpdateSettersBuilder<ContractInstallmentEntity>> Update()
    {
        return setters => setters
            .SetProperty(installment => installment.Description, item.Description)
            .SetProperty(installment => installment.DueDate, item.DueDate)
            .SetProperty(installment => installment.Amount, item.Amount)
            .SetProperty(installment => installment.AmountText, item.AmountText)
            .SetProperty(installment => installment.UpdatedDate, now)
            .SetProperty(installment => installment.ModifiedBy, modifiedBy);
    }
}

public sealed class AddInstallmentSpec(
    Guid contractId,
    UpdateContractEndpoint.InstallmentItemRequest item,
    DateTime now,
    string createdBy) : IAddSpecification<ContractInstallmentEntity>
{
    public ContractInstallmentEntity Add()
    {
        return new ContractInstallmentEntity
        {
            Id = Guid.CreateVersion7(),
            ContractId = contractId,
            Description = item.Description,
            DueDate = item.DueDate,
            Amount = item.Amount,
            AmountText = item.AmountText,
            CreatedBy = createdBy,
            CreatedDate = now
        };
    }
}
