using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TerraLink.UseCase.Finance.Features.Overview.Endpoints;

namespace TerraLink.UseCase.Finance.Features.Overview;

public sealed class OverviewModule : IModule
{
    public static void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/finance")
            .WithTags("Finance Overview")
            .WithGroupName("finance-module")
            .RequireAuthorization();

        group.MapEndpoint<GetFinanceOverviewEndpoint>();
    }

    public static void AddServices(IServiceCollection services) { }
}
