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
    public sealed partial class GetFollowUpCallDetailsEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/{id}", Handle)
               .Produces<Response>(StatusCodes.Status200OK)
               .AddLogging()
               .WithName("GetFollowUpCallById")
               .WithSummary("Get follow-up call by id")
               .RequirePermission(Permissions.FollowUpRecords.View);
        }

        public static async Task<IResult> Handle(
            [FromRoute] Guid clientId,
            [FromRoute] Guid id,
            IOperation dispatcher,
            CancellationToken cancellationToken)
        {
            var getResult = await dispatcher.ExecuteAsync(new GetFollowUpCallDetailsOperation.Request(clientId, id), cancellationToken);

            if (getResult.IsError)
            {
                return getResult.FirstError.ToUnprocessableEntity();
            }

                        Response response = ObjectMapper.Map<GetFollowUpCallDetailsOperation.Response, Response>(getResult.Value);

            return TypedResults.Ok(response);
        }
    }
}
