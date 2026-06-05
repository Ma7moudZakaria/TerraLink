using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.Asset.Features.Buildings.Endpoints;

namespace TerraLink.UseCase.Asset.Features.Buildings
{
    public sealed class BuildingModule : IModule
    {
        public static void AddRoutes(IEndpointRouteBuilder app)
        {
            RouteGroupBuilder group = app.MapGroup("/api/buildings")
                                         .WithTags("Buildings")
                                         .WithGroupName("asset-module")
                                         .RequireAuthorization(); // All endpoints require authentication

            group.MapEndpoint<GetBuildingsEndpoint>()
                 .MapEndpoint<GetBuildingDetailsEndpoint>()
                 .MapEndpoint<GetBuildingsDropdownEndpoint>()
                 .MapEndpoint<CreateBuildingEndpoint>()
                 .MapEndpoint<UpdateBuildingEndpoint>()
                 .MapEndpoint<DeleteBuildingEndpoint>();

        }
    }
}
