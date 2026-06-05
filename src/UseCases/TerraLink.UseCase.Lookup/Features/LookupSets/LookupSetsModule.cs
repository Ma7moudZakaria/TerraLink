using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TerraLink.UseCase.Lookup.Features.LookupSets.Endpoints;

namespace TerraLink.UseCase.Lookup.Features.LookupSets;

public sealed class LookupSetsModule : IModule
{
    public static void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/lookups")
            .WithTags("Lookup Sets")
            .WithGroupName("lookup-module")
            .RequireAuthorization();

        group.MapEndpoint<AddLookupSetEndpoint>()
             .MapEndpoint<GetAllLookupSetsEndpoint>()
             .MapEndpoint<GetLookupSetByCodeEndpoint>();
    }

    public static void AddServices(IServiceCollection services) { }
}
