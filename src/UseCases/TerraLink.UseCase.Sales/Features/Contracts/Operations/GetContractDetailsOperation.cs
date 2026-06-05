using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Contracts.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Sales.Features.Contracts.Operations;

public sealed partial class GetContractDetailsOperation(IRepository<ContractEntity> contracts, IMapper mapper)
    : IOperationHandler<GetContractDetailsOperation.Request, GetContractDetailsOperation.Response>
{
    public async Task<ErrorOr<GetContractDetailsOperation.Response>> HandleAsync(GetContractDetailsOperation.Request request, CancellationToken ct = default)
    {
        if (request.Id == Guid.Empty)
        {
            return Error.Validation(ErrorCode.NotValidId, "Id is not correct");
        }

        ContractEntity? contract = await contracts.GetAsync(new ContractByIdSpec(request.Id, includeDetails: true), ct);
        if (contract is null)
        {
            return Error.Validation(ErrorCode.NoItemExist, "There is no Contract exist");
        }

        return mapper.Map<ContractEntity, GetContractDetailsOperation.Response>(contract);
    }
}
