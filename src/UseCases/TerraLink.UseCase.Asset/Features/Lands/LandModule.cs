using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.Asset.Features.Lands.Endpoints;

namespace TerraLink.UseCase.Asset.Features.Lands
{
    public sealed class LandModule : IModule
    {
        public static void AddRoutes(IEndpointRouteBuilder app)
        {
            RouteGroupBuilder group = app.MapGroup("/api/lands")
                                         .WithTags("Lands")
                                         .WithGroupName("asset-module")
                                         .RequireAuthorization(); // All endpoints require authentication

            group.MapEndpoint<GetLandsEndpoint>()
                 .MapEndpoint<GetLandDetailsEndpoint>()
                 .MapEndpoint<GetLandsDropdownEndpoint>()
                 .MapEndpoint<CreateLandEndpoint>()
                 .MapEndpoint<UpdateLandEndpoint>()
                 .MapEndpoint<DeleteLandEndpoint>();
        }
    }
}
