using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.FollowUpCalls.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Sales.Features.FollowUpCalls.Operations;

public sealed partial class DeleteFollowUpCallOperation(
    IRepository<FollowUpCallEntity> followUpCalls,
    IRepository<ClientEntity> clients)
    : IOperationHandler<DeleteFollowUpCallOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(DeleteFollowUpCallOperation.Request request, CancellationToken ct = default)
    {
        if (request.Id == Guid.Empty)
        {
            return Error.Validation(ErrorCode.NotValidId, "Id is not correct");
        }

        Error? validationError = await CreateFollowUpCallOperation.ValidateClientAndCallAsync(clients, followUpCalls, request.ClientId, request.Id, ct);
        if (validationError is not null) return validationError.Value;

        return await followUpCalls.UpdateAsync(new FollowUpCallDetailsSpec(request.ClientId, request.Id), new SoftDeleteFollowUpCallUpdateSpec(), ct) > 0
            ? new Success()
            : Errors.DeleteFaild;
    }
}
