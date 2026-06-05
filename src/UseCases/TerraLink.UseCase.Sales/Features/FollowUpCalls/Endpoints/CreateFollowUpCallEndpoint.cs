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
    public sealed partial class CreateFollowUpCallEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapPost("/", Handle)
               .Produces<Response>(StatusCodes.Status201Created)
               .AddLogging()
               .WithName("CreateFollowUpCall")
               .WithSummary("Create follow-up call")
               .RequirePermission(Permissions.FollowUpRecords.Create);
        }

        public static async Task<IResult> Handle(
            [FromRoute] Guid clientId,
            [FromBody] Request request,
            IOperation dispatcher,
            CancellationToken cancellationToken)
        {
            var createResult = await dispatcher.ExecuteAsync(new CreateFollowUpCallOperation.Request(clientId, request), cancellationToken);

            if (createResult.IsError)
            {
                return createResult.FirstError.ToUnprocessableEntity();
            }

            return TypedResults.Created($"/api/clients/{clientId}/follow-up-calls", Response.Default);
        }
    }
}
