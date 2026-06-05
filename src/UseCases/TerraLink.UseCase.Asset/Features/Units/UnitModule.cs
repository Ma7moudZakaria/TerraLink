using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.Asset.Features.Units.Endpoints;

namespace TerraLink.UseCase.Asset.Features.Units
{
    public sealed class UnitModule : IModule
    {
        public static void AddRoutes(IEndpointRouteBuilder app)
        {
            RouteGroupBuilder group = app.MapGroup("/api/units")
                                         .WithTags("Units")
                                         .WithGroupName("asset-module")
                                         .RequireAuthorization(); // All endpoints require authentication

            group.MapEndpoint<GetUnitsEndpoint>()
                 .MapEndpoint<GetUnitDetailsEndpoint>()
                 .MapEndpoint<GetUnitsDropdownEndpoint>()
                 .MapEndpoint<CreateUnitEndpoint>()
                 .MapEndpoint<UpdateUnitEndpoint>()
                 .MapEndpoint<DeleteUnitEndpoint>();
        }
    }
}
