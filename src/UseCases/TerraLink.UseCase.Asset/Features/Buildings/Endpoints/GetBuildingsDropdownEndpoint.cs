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
    public sealed partial class GetBuildingsDropdownEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/dropdown", Handle)
             .Produces<List<Response>>(StatusCodes.Status200OK)
             .AddLogging()
             .WithName("GetBuildingsDropdown")
             .WithSummary("Get Buildings for Dropdown")
             .WithDescription("Returns a simplified list of buildings (Id and Name) for use in dropdown/select controls. Optionally filter by landId.")
             .RequirePermission(Permissions.Assets.View);
        }

        public static async Task<IResult> Handle([FromQuery] Guid? landId, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var buildings = await dispatcher.ExecuteAsync(new GetBuildingsDropdownOperation.Request(landId), cancellationToken);

            if (buildings.IsError)
            {
                return buildings.ToProblem();
            }

            List<Response> response = ObjectMapper.MapList<GetBuildingsDropdownOperation.Response, Response>(buildings.Value);

            return TypedResults.Ok(response);
        }
    }
}
