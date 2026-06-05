using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.FollowUpCalls.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Sales.Features.FollowUpCalls.Operations;

public sealed partial class GetFollowUpCallDetailsOperation(
    IRepository<FollowUpCallEntity> followUpCalls,
    IRepository<ClientEntity> clients,
    IMapper mapper)
    : IOperationHandler<GetFollowUpCallDetailsOperation.Request, GetFollowUpCallDetailsOperation.Response>
{
    public async Task<ErrorOr<GetFollowUpCallDetailsOperation.Response>> HandleAsync(GetFollowUpCallDetailsOperation.Request request, CancellationToken ct = default)
    {
        Error? validationError = await CreateFollowUpCallOperation.ValidateClientAndCallAsync(clients, followUpCalls, request.ClientId, request.Id, ct);
        if (validationError is not null) return validationError.Value;

        FollowUpCallEntity? call = await followUpCalls.GetAsync(new FollowUpCallDetailsSpec(request.ClientId, request.Id), ct);
        if (call is null)
        {
            return Error.Validation(ErrorCode.NoItemExist, "There is no follow-up call");
        }

        return mapper.Map<FollowUpCallEntity, GetFollowUpCallDetailsOperation.Response>(call);
    }
}
