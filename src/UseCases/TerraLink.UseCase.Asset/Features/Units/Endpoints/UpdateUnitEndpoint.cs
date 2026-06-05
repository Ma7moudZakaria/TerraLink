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
using TerraLink.UseCase.Asset.Features.Units.Operations;

namespace TerraLink.UseCase.Asset.Features.Units.Endpoints
{
    public sealed partial class UpdateUnitEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapPut("/{id}/update", Handle)
            .Produces<Response>(StatusCodes.Status200OK)
            .AddValidator<Request>()
            .AddLogging()
            .WithName("UpdateUnit")
            .WithSummary("Update Unit")
            .RequirePermission(Permissions.Assets.Edit); // Permission filter
        }
        public static async Task<IResult> Handle([FromRoute] Guid id, [FromBody] Request request, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var updateResult = await dispatcher.ExecuteAsync(new UpdateUnitOperation.Request(id, request), cancellationToken);

            if (updateResult.IsError)
            {
                return updateResult.FirstError.ToBusinessFailure();
            }
            return TypedResults.Ok(Response.Default);

        }
    }
}
