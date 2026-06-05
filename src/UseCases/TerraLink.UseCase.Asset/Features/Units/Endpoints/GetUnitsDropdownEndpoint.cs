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
using TerraLink.UseCase.Asset.Features.Units.Operations;

namespace TerraLink.UseCase.Asset.Features.Units.Endpoints
{
    public sealed partial class GetUnitsDropdownEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/dropdown", Handle)
             .Produces<List<Response>>(StatusCodes.Status200OK)
             .AddLogging()
             .WithName("GetUnitsDropdown")
             .WithSummary("Get Units for Dropdown")
             .WithDescription("Returns a simplified list of units (Id and Name) for use in dropdown/select controls. Optionally filter by buildingId or landId.")
             .RequirePermission(Permissions.Assets.View);
        }

        public static async Task<IResult> Handle([FromQuery] Guid? buildingId, [FromQuery] Guid? landId, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var units = await dispatcher.ExecuteAsync(new GetUnitsDropdownOperation.Request(buildingId, landId), cancellationToken);

            if (units.IsError)
            {
                return units.ToProblem();
            }

            List<Response> response = ObjectMapper.MapList<GetUnitsDropdownOperation.Response, Response>(units.Value);

            return TypedResults.Ok(response);
        }
    }
}
