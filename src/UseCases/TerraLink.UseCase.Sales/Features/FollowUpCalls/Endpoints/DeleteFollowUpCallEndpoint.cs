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
    public sealed partial class DeleteFollowUpCallEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapDelete("/{id}", Handle)
               .Produces<Response>(StatusCodes.Status200OK)
               .AddLogging()
               .WithName("DeleteFollowUpCall")
               .WithSummary("Delete follow-up call")
               .RequirePermission(Permissions.FollowUpRecords.Delete);
        }

        public static async Task<IResult> Handle(
            [FromRoute] Guid clientId,
            [FromRoute] Guid id,
            IOperation dispatcher,
            CancellationToken cancellationToken)
        {
            var deleteResult = await dispatcher.ExecuteAsync(new DeleteFollowUpCallOperation.Request(clientId, id), cancellationToken);

            if (deleteResult.IsError)
            {
                return deleteResult.FirstError.ToUnprocessableEntity();
            }
            return TypedResults.Ok(Response.Default);
        }
    }
}
