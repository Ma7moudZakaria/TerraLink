using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using LowCodeHub.Permissions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using TerraLink.UseCase.Dashboard.Features.Overview.Operations;

namespace TerraLink.UseCase.Dashboard.Features.Overview.Endpoints;

public sealed partial class GetUserDashboardOverviewEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("/userdashboard/overview", Handle)
           .Produces<Response>(StatusCodes.Status200OK)
           .RequirePermission(Permissions.Dashboard.View)
           .AddLogging()
           .WithName("GetUserDashboardOverview")
           .WithSummary("Get User Dashboard Overview");
    }

    public static async Task<IResult> Handle(IOperation dispatcher, CancellationToken ct)
    {
        var result = await dispatcher.ExecuteAsync(new GetUserDashboardOverviewOperation.Request(), ct);
                    Response response = ObjectMapper.Map<GetUserDashboardOverviewOperation.Response, Response>(result.Value);

            return TypedResults.Ok(response);
    }
}
