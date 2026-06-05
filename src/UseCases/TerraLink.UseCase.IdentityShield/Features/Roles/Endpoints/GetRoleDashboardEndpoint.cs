using ErrorOr;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using LowCodeHub.Permissions;
using TerraLink.UseCase.IdentityShield.Features.Roles.Operations;

namespace TerraLink.UseCase.IdentityShield.Features.Roles.Endpoints
{
    public sealed partial class GetRoleDashboardEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/dashboard", Handle)
                    .Produces<Response>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                   .WithName("GetRoleDashboard")
                 .WithSummary("Get Role Dashboard")
                 .WithDescription("Retrieve dashboard statistics including role count, user count, and role distribution")
             .RequirePermission(Permissions.Dashboard.View) // Permission filter - use Dashboard permission for dashboard endpoint
            .AddLogging();
        }

        public static async Task<IResult> Handle(IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new GetRoleDashboardOperation.Request(), cancellationToken);

            if (result.IsError)
            {
                return result.FirstError.ToUnprocessableEntity();
            }

                        Response response = ObjectMapper.Map<GetRoleDashboardOperation.Response, Response>(result.Value);

            return TypedResults.Ok(response);
        }
    }
}
