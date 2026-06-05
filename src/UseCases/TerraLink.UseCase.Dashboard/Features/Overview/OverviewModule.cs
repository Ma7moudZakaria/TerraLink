using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TerraLink.UseCase.Dashboard.Features.Overview.Endpoints;

namespace TerraLink.UseCase.Dashboard.Features.Overview;

public sealed class OverviewModule : IModule
{
    public static void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/dashboard")
            .WithTags("Dashboard")
            .WithGroupName("dashboard-module")
            .RequireAuthorization();

        group.MapEndpoint<GetDashboardOverviewEndpoint>()
             .MapEndpoint<GetUserDashboardOverviewEndpoint>();
    }
}
