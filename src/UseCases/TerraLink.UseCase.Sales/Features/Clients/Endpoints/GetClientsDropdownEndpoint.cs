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
using TerraLink.UseCase.Sales.Features.Clients.Operations;

namespace TerraLink.UseCase.Sales.Features.Clients.Endpoints
{
    public sealed partial class GetClientsDropdownEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/dropdown", Handle)
             .Produces<List<Response>>(StatusCodes.Status200OK)
             .AddLogging()
             .WithName("GetClientsDropdown")
             .WithSummary("Get Clients for Dropdown")
             .WithDescription("Returns a simplified list of clients (Id and Name) for use in dropdown/select controls. Optionally filter by clientId.")
             .RequirePermission(Permissions.Clients.View);
        }

        public static async Task<IResult> Handle([FromQuery] Guid? clientId, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var clients = await dispatcher.ExecuteAsync(new GetClientsDropdownOperation.Request(clientId), cancellationToken);

            if (clients.IsError)
            {
                return clients.ToProblem();
            }

            List<Response> response = ObjectMapper.MapList<GetClientsDropdownOperation.Response, Response>(clients.Value);

            return TypedResults.Ok(response);
        }
    }
}
