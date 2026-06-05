using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.Asset.Features.Overview.Endpoints;

namespace TerraLink.UseCase.Asset.Features.Overview
{
    public sealed class AssetOverviewModule : IModule
    {
        public static void AddRoutes(IEndpointRouteBuilder app)
        {
            RouteGroupBuilder group = app.MapGroup("/api/assets")
                                         .WithTags("Assets-Overview")
                                         .WithGroupName("asset-module")
                                         .RequireAuthorization();

            group.MapEndpoint<GetAssetOverviewEndpoint>();
        }
    }
}
