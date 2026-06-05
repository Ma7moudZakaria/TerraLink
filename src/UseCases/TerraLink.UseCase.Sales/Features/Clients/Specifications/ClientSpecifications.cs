using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Clients.Endpoints;

namespace TerraLink.UseCase.Sales.Features.Clients.Specifications;

public sealed class CreateClientAddSpec(CreateClientEndpoint.Request request, string code) : IAddSpecification<ClientEntity>
{
    public ClientEntity Add()
    {
        return ObjectMapper.Map<CreateClientEndpoint.Request, ClientEntity>(request)
            .Set(entity => entity.Id, Guid.CreateVersion7())
            .Set(entity => entity.Code, code)
            .Set(entity => entity.CreatedBy, "SYS")
            .Set(entity => entity.CreatedDate, DateTime.UtcNow);
    }
}

public sealed class SoftDeleteClientUpdateSpec : IUpdateSpecification<ClientEntity>
{
    public Action<UpdateSettersBuilder<ClientEntity>> Update()
    {
        return setters => setters.SetProperty(entity => entity.IsDeleted, true);
    }
}

public sealed class ClientByIdSpec(Guid id) : ISpecification<ClientEntity>
{
    public IQueryable<ClientEntity> Where(IQueryable<ClientEntity> query)
    {
        return query.Where(entity => entity.Id == id && !entity.IsDeleted);
    }
}

public sealed class ClientByNationalIdSpec(string nationalId) : ISpecification<ClientEntity>
{
    public IQueryable<ClientEntity> Where(IQueryable<ClientEntity> query)
    {
        return query.Where(entity => entity.NationalId == nationalId && !entity.IsDeleted);
    }
}

public sealed class UpdateClientFieldsSpec : IUpdateSpecification<ClientEntity>
{
    public required string Name { get; init; }
    public required string Phone { get; init; }
    public required string Email { get; init; }
    public required string Address { get; init; }
    public required string NationalId { get; init; }

    public Action<UpdateSettersBuilder<ClientEntity>> Update()
    {
        return setters => setters
            .SetProperty(entity => entity.Name, Name)
            .SetProperty(entity => entity.Phone, Phone)
            .SetProperty(entity => entity.Email, Email)
            .SetProperty(entity => entity.Address, Address)
            .SetProperty(entity => entity.NationalId, NationalId)
            .SetProperty(entity => entity.UpdatedDate, DateTime.UtcNow);
    }
}

public sealed class ClientDetailsSpec(Guid id) : ISpecification<ClientEntity>
{
    public IQueryable<ClientEntity> Where(IQueryable<ClientEntity> query)
    {
        return IncludeDetails(query)
            .Where(entity => entity.Id == id && !entity.IsDeleted);
    }

    internal static IQueryable<ClientEntity> IncludeDetails(IQueryable<ClientEntity> query)
    {
        return query
            .Include(entity => entity.ClientContracts)
                .ThenInclude(clientContract => clientContract.Contract)
                    .ThenInclude(contract => contract.Unit)
            .Include(entity => entity.ClientContracts)
                .ThenInclude(clientContract => clientContract.Contract)
                    .ThenInclude(contract => contract.Building)
            .Include(entity => entity.ClientContracts)
                .ThenInclude(clientContract => clientContract.Contract)
                    .ThenInclude(contract => contract.Land)
            .Include(entity => entity.ClientContracts)
                .ThenInclude(clientContract => clientContract.Contract)
                    .ThenInclude(contract => contract.Installments)
                        .ThenInclude(installment => installment.IncomingPayments);
    }
}

public sealed class ClientsListSpec(GetAllClientsEndpoint.Request request) : ISpecification<ClientEntity>
{
    public IQueryable<ClientEntity> Where(IQueryable<ClientEntity> query)
    {
        query = query.Where(entity => !entity.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.UnitNumber))
        {
            query = query.Where(entity => entity.ClientContracts.Any(clientContract =>
                clientContract.Contract.Unit != null && clientContract.Contract.Unit.Number == request.UnitNumber));
        }

        if (!string.IsNullOrWhiteSpace(request.BuildingName))
        {
            query = query.Where(entity => entity.ClientContracts.Any(clientContract =>
                clientContract.Contract.Building != null && clientContract.Contract.Building.Name.Contains(request.BuildingName)));
        }

        if (!string.IsNullOrWhiteSpace(request.LandName))
        {
            query = query.Where(entity => entity.ClientContracts.Any(clientContract =>
                clientContract.Contract.Land != null && clientContract.Contract.Land.Name.Contains(request.LandName)));
        }

        if (request.FloorNumber.HasValue)
        {
            query = query.Where(entity => entity.ClientContracts.Any(clientContract =>
                clientContract.Contract.Unit != null && clientContract.Contract.Unit.FloorNumber == request.FloorNumber.Value));
        }

        return query.OrderByDescending(entity => entity.CreatedDate);
    }
}

public sealed class ActiveClientsSpec(Guid? clientId) : ISpecification<ClientEntity>
{
    public IQueryable<ClientEntity> Where(IQueryable<ClientEntity> query)
    {
        query = query.Where(entity => !entity.IsDeleted);

        if (clientId.HasValue)
        {
            query = query.Where(entity => entity.Id == clientId.Value);
        }

        return query.OrderBy(entity => entity.Name);
    }
}
