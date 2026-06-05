using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using LowCodeHub.Permissions;
using TerraLink.UseCase.Sales.Features.Clients.Operations;

namespace TerraLink.UseCase.Sales.Features.Clients.Endpoints
{
    public sealed partial class GetClientOverviewEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/overview", Handle)
               .Produces<Response>(StatusCodes.Status200OK)
               .WithName("GetClientsOverview")
               .WithSummary("Get Clients Overview")
               .RequirePermission(Permissions.Clients.View)
               .AddLogging();
        }

        public static async Task<IResult> Handle(IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new GetClientOverviewOperation.Request(), cancellationToken);
            if (result.IsError)
            {
                return result.ToProblem();
            }

            Response response = ObjectMapper.Map<GetClientOverviewOperation.Response, Response>(result.Value);

            return TypedResults.Ok(response);
        }
    }
}
