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
using TerraLink.UseCase.Asset.Features.Lands.Operations;

namespace TerraLink.UseCase.Asset.Features.Lands.Endpoints
{
    public sealed partial class DeleteLandEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapDelete("/{id}/delete", Handle)
           .Produces<Response>(StatusCodes.Status200OK)
           .AddLogging()
           .WithName("DeleteLand")
           .WithSummary("Delete Land")
           .RequirePermission(Permissions.Assets.Delete); // Permission filter
        }

        public static async Task<IResult> Handle([FromRoute] Guid id, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var deleteResult = await dispatcher.ExecuteAsync(new DeleteLandOperation.Request(id), cancellationToken);

            if (deleteResult.IsError)
            {
                return deleteResult.FirstError.ToBusinessFailure();
            }
            return TypedResults.Ok(Response.Default);

        }
    }
}
