using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Clients.Specifications;
using TerraLink.UseCase.Sales.Features.FollowUpCalls.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Sales.Features.FollowUpCalls.Operations;

public sealed partial class UpdateFollowUpCallOperation(
    IRepository<FollowUpCallEntity> followUpCalls,
    IRepository<ClientEntity> clients)
    : IOperationHandler<UpdateFollowUpCallOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(UpdateFollowUpCallOperation.Request request, CancellationToken ct = default)
    {
        if (request.Id == Guid.Empty)
        {
            return Error.Validation(ErrorCode.NotValidId, "Id is not correct");
        }

        Error? validationError = await CreateFollowUpCallOperation.ValidateClientAndCallAsync(clients, followUpCalls, request.ClientId, request.Id, ct);
        if (validationError is not null) return validationError.Value;

        validationError = await CreateFollowUpCallOperation.ValidateClientAndRequestAsync(clients, request.ClientId, request.Payload.CallDate, request.Payload.CallerName, ct);
        if (validationError is not null) return validationError.Value;

        var update = new UpdateFollowUpCallFieldsSpec
        {
            CallDate = request.Payload.CallDate,
            CallerName = request.Payload.CallerName,
            Note = request.Payload.Note,
            ClientId = request.ClientId
        };

        return await followUpCalls.UpdateAsync(new FollowUpCallDetailsSpec(request.ClientId, request.Id), update, ct) > 0
            ? new Success()
            : Errors.UpdateFaild;
    }
}
