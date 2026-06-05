using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.FollowUpCalls.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Sales.Features.FollowUpCalls.Operations;

public sealed partial class GetFollowUpCallsOperation(
    IRepository<FollowUpCallEntity> followUpCalls,
    IRepository<ClientEntity> clients,
    IMapper mapper)
    : IOperationHandler<GetFollowUpCallsOperation.Request, List<GetFollowUpCallsOperation.Response>>
{
    public async Task<ErrorOr<List<GetFollowUpCallsOperation.Response>>> HandleAsync(GetFollowUpCallsOperation.Request request, CancellationToken ct = default)
    {
        Error? validationError = await CreateFollowUpCallOperation.ValidateClientAsync(clients, request.ClientId, ct);
        if (validationError is not null) return validationError.Value;

        IReadOnlyList<FollowUpCallEntity> calls = await followUpCalls.ListAsync(new FollowUpCallsListSpec(request.ClientId), ct);

        return calls.Select(mapper.Map<FollowUpCallEntity, GetFollowUpCallsOperation.Response>).ToList();
    }
}
