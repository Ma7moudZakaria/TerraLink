using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.FollowUpCalls.Operations;

namespace TerraLink.UseCase.Sales.Features.FollowUpCalls.Mappers;

public sealed class FollowUpCallDetailsResponseMapper : IMapHandler<FollowUpCallEntity, GetFollowUpCallDetailsOperation.Response>
{
    public GetFollowUpCallDetailsOperation.Response Handler(FollowUpCallEntity source)
    {
        return new GetFollowUpCallDetailsOperation.Response
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
