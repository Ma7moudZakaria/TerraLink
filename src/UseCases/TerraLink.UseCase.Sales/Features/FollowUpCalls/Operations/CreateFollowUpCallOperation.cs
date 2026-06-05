using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Clients.Specifications;
using TerraLink.UseCase.Sales.Features.FollowUpCalls.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Sales.Features.FollowUpCalls.Operations;

public sealed partial class CreateFollowUpCallOperation(
    IRepository<FollowUpCallEntity> followUpCalls,
    IRepository<ClientEntity> clients)
    : IOperationHandler<CreateFollowUpCallOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(CreateFollowUpCallOperation.Request request, CancellationToken ct = default)
    {
        Error? validationError = await ValidateClientAndRequestAsync(clients, request.ClientId, request.Payload.CallDate, request.Payload.CallerName, ct);
        if (validationError is not null) return validationError.Value;

        followUpCalls.Add(new CreateFollowUpCallAddSpec(request.ClientId, request.Payload));

        return await followUpCalls.SaveChangesAsync(ct) > 0
            ? new Success()
            : Errors.CreateFaild;
    }

    internal static async Task<Error?> ValidateClientAndRequestAsync(
        IRepository<ClientEntity> clients,
        Guid clientId,
        DateTime callDate,
        string callerName,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(callerName))
        {
            return Error.Validation(ErrorCode.NoItemCorrect, "CallerName is required");
        }

        if (callDate == default)
        {
            return Error.Validation(ErrorCode.NoItemCorrect, "CallDate is required");
        }

        return await ValidateClientAsync(clients, clientId, ct);
    }

    internal static async Task<Error?> ValidateClientAsync(IRepository<ClientEntity> clients, Guid clientId, CancellationToken ct)
    {
        if (clientId == Guid.Empty)
        {
            return Error.Validation(ErrorCode.NotValidId, "ClientId is not correct");
        }

        return await clients.CountAsync(new ClientByIdSpec(clientId), ct) > 0
            ? null
            : Error.Validation(ErrorCode.NoItemExist, "Client does not exist");
    }

    internal static async Task<Error?> ValidateClientAndCallAsync(
        IRepository<ClientEntity> clients,
        IRepository<FollowUpCallEntity> followUpCalls,
        Guid clientId,
        Guid callId,
        CancellationToken ct)
    {
        Error? clientValidation = await ValidateClientAsync(clients, clientId, ct);
        if (clientValidation is not null)
        {
            return clientValidation.Value;
        }

        return await followUpCalls.CountAsync(new FollowUpCallDetailsSpec(clientId, callId), ct) > 0
            ? null
            : Error.Validation(ErrorCode.NoItemExist, "There is no follow-up call for this client");
    }
}
