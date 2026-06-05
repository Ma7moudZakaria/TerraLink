using ErrorOr;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using LowCodeHub.Permissions;
using TerraLink.UseCase.Sales.Features.FollowUpCalls.Operations;

namespace TerraLink.UseCase.Sales.Features.FollowUpCalls.Endpoints
{
    public sealed partial class GetAllFollowUpCallsEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/", Handle)
               .Produces<List<Response>>(StatusCodes.Status200OK)
               .AddLogging()
               .WithName("GetAllFollowUpCalls")
               .WithSummary("Get all follow-up calls")
               .RequirePermission(Permissions.FollowUpRecords.View);
        }

        public static async Task<IResult> Handle(
            [FromRoute] Guid clientId,
            IOperation dispatcher,
            CancellationToken cancellationToken)
        {
            var getResult = await dispatcher.ExecuteAsync(new GetFollowUpCallsOperation.Request(clientId), cancellationToken);

            if (getResult.IsError)
            {
                return getResult.FirstError.ToUnprocessableEntity();
            }

                        List<Response> response = ObjectMapper.MapList<GetFollowUpCallsOperation.Response, Response>(getResult.Value);

            return TypedResults.Ok(response);
        }
    }
}
