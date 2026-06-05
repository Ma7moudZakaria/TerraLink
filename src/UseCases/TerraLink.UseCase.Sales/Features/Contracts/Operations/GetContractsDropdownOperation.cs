using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using LowCodeHub.QueryableExtensions.Transactions;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Interfaces;
using TerraLink.Domain.Interfaces.Services;
using TerraLink.Domain.Persistence;
using TerraLink.UseCase.Sales.Features.Contracts.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Sales.Features.Contracts.Operations;

public sealed partial class GetContractsDropdownOperation(IRepository<ContractEntity> contracts)
    : IOperationHandler<GetContractsDropdownOperation.Request, List<GetContractsDropdownOperation.Response>>
{
    public async Task<ErrorOr<List<GetContractsDropdownOperation.Response>>> HandleAsync(GetContractsDropdownOperation.Request request, CancellationToken ct = default)
    {
        IReadOnlyList<ContractEntity> result = await contracts.ListAsync(new ContractsDropdownSpec(request.ContractTypeId), ct);

        return result.Select(contract => new GetContractsDropdownOperation.Response(contract.Id, contract.ContractNumber)).ToList();
    }
}
