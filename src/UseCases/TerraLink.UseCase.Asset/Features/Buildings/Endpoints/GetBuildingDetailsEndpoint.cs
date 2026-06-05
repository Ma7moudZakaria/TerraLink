using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using LowCodeHub.Permissions;
using TerraLink.UseCase.Asset.Features.Buildings.Operations;

namespace TerraLink.UseCase.Asset.Features.Buildings.Endpoints
{
    public sealed partial class GetBuildingDetailsEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/{id}", Handle)
            .Produces<Response>(StatusCodes.Status200OK)
            .AddLogging()
            .WithName("GetBuildingById")
            .WithSummary("Get Building By ID")
            .RequirePermission(Permissions.Assets.View); // Permission filter
        }
        public static async Task<IResult> Handle([FromRoute] Guid id, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new GetBuildingDetailsOperation.Request(id), cancellationToken);

            if (result.IsError)
            {
                return result.ToProblem();
            }

            Response response = ObjectMapper.Map<GetBuildingDetailsOperation.Response, Response>(result.Value);

            return TypedResults.Ok(response);

        }
    }
}
