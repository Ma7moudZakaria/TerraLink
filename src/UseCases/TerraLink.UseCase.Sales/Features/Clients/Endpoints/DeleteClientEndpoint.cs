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
using TerraLink.UseCase.Sales.Features.Clients.Operations;

namespace TerraLink.UseCase.Sales.Features.Clients.Endpoints
{
    public sealed partial class DeleteClientEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapDelete("/{id}/delete", Handle)
               .Produces<Response>(StatusCodes.Status200OK)
               .AddLogging()
               .WithName("DeleteClient")
               .WithSummary("Delete Client")
               .RequirePermission(Permissions.Clients.Delete);
        }

        public static async Task<IResult> Handle([FromRoute] Guid id,
                                                 IOperation dispatcher,
                                                 CancellationToken cancellationToken)
        {
            var deleteResult = await dispatcher.ExecuteAsync(new DeleteClientOperation.Request(id), cancellationToken);

            if (deleteResult.IsError)
            {
                return deleteResult.FirstError.ToUnprocessableEntity();
            }
            return TypedResults.Ok(Response.Default);
        }
    }
}
