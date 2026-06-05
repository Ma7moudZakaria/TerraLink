using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.Sales.Features.Dashboard.Endpoints;

namespace TerraLink.UseCase.Sales.Features.Dashboard
{
    public sealed class DashboardModule : IModule
    {
        public static void AddRoutes(IEndpointRouteBuilder app)
        {
            RouteGroupBuilder group = app.MapGroup("/api/dashboard")
                                         .WithTags("Dashboard")
                                         .WithGroupName("sales-module")
                                         .RequireAuthorization();

            group.MapEndpoint<GetDashboardOverviewEndpoint>();
        }
    }
}
