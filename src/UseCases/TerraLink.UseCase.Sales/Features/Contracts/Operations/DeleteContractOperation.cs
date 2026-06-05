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

public sealed partial class DeleteContractOperation(IRepository<ContractEntity> contracts, ICurrentUserService currentUserService)
    : IOperationHandler<DeleteContractOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(DeleteContractOperation.Request request, CancellationToken ct = default)
    {
        if (request.Id == Guid.Empty)
        {
            return Error.Validation(ErrorCode.NotValidId, "Id is not correct");
        }

        string userName = currentUserService.UserName ?? "SYSTEM";

        return await contracts.UpdateAsync(
                new ContractByIdSpec(request.Id),
                new SoftDeleteContractSpec(DateTime.UtcNow, userName),
                ct) > 0
            ? new Success()
            : Errors.DeleteFaild;
    }
}
