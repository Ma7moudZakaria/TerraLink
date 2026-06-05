using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using LowCodeHub.Permissions;
using TerraLink.UseCase.Asset.Features.Lands.Operations;

namespace TerraLink.UseCase.Asset.Features.Lands.Endpoints
{
    public sealed partial class GetLandsDropdownEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/dropdown", Handle)
             .Produces<List<Response>>(StatusCodes.Status200OK)
             .AddLogging()
             .WithName("GetLandsDropdown")
             .WithSummary("Get Lands for Dropdown")
             .WithDescription("Returns a simplified list of lands (Id and Name) for use in dropdown/select controls")
             .RequirePermission(Permissions.Assets.View);
        }

        public static async Task<IResult> Handle(IOperation dispatcher, CancellationToken cancellationToken)
        {
            var lands = await dispatcher.ExecuteAsync(new GetLandsDropdownOperation.Request(), cancellationToken);

            if (lands.IsError)
            {
                return lands.ToProblem();
            }

            List<Response> response = ObjectMapper.MapList<GetLandsDropdownOperation.Response, Response>(lands.Value);

            return TypedResults.Ok(response);
        }
    }
}
