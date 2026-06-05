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
using TerraLink.UseCase.Sales.Features.Contracts.Operations;

namespace TerraLink.UseCase.Sales.Features.Contracts.Endpoints
{
    public sealed partial class UpdateContractEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapPut("/{id}/update", Handle)
               .Produces<Response>(StatusCodes.Status200OK)
               .AddLogging()
               .WithName("UpdateContract")
               .WithSummary("Update Contract")
               .RequirePermission(Permissions.Contracts.Edit);
        }

        public static async Task<IResult> Handle([FromRoute] Guid id,
                                                 [FromBody] Request request,
                                                 IOperation dispatcher,
                                                 CancellationToken cancellationToken)
        {
            var updateResult = await dispatcher.ExecuteAsync(new UpdateContractOperation.Request(id, request), cancellationToken);

            if (updateResult.IsError)
            {
                return updateResult.FirstError.ToUnprocessableEntity();
            }
            return TypedResults.Ok(Response.Default);
        }
    }
}
