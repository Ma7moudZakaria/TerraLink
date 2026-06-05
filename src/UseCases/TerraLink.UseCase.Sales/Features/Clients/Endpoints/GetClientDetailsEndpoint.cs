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
    public sealed partial class GetClientDetailsEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/{id}", Handle)
               .Produces<Response>(StatusCodes.Status200OK)
               .AddLogging()
               .WithName("GetClientById")
               .WithSummary("Get Client By ID")
               .RequirePermission(Permissions.Clients.View); // Permission filter
        }

        public static async Task<IResult> Handle([FromRoute] Guid id,
                                                 IOperation dispatcher,
                                                 CancellationToken cancellationToken)
        {
            var getDetailsResult = await dispatcher.ExecuteAsync(new GetClientDetailsOperation.Request(id), cancellationToken);

            if (getDetailsResult.IsError)
            {
                return getDetailsResult.FirstError.ToUnprocessableEntity();
            }

                        Response response = ObjectMapper.Map<GetClientDetailsOperation.Response, Response>(getDetailsResult.Value);

            return TypedResults.Ok(response);
        }
    }
}
