using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.FollowUpCalls.Operations;

namespace TerraLink.UseCase.Sales.Features.FollowUpCalls.Mappers;

public sealed class FollowUpCallListResponseMapper : IMapHandler<FollowUpCallEntity, GetFollowUpCallsOperation.Response>
{
    public GetFollowUpCallsOperation.Response Handler(FollowUpCallEntity source)
    {
        return new GetFollowUpCallsOperation.Response
        {
            FollowCallId = source.Id,
            CallDate = source.CallDate,
            CallerName = source.CallerName,
            Note = source.Note,
            ClientId = source.ClientId,
            ClientName = source.Client.Name
        };
    }
}
