using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TerraLink.UseCase.Lookup.Features.LookupItems.Endpoints;

namespace TerraLink.UseCase.Lookup.Features.LookupItems;

public sealed class LookupItemsModule : IModule
{
    public static void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/lookups")
            .WithTags("Lookup Items")
            .WithGroupName("lookup-module")
            .RequireAuthorization();

        group.MapEndpoint<AddLookupItemEndpoint>()
             .MapEndpoint<GetLookupItemsBySetIdEndpoint>()
             .MapEndpoint<GetLookupItemByCodeEndpoint>();
    }

    public static void AddServices(IServiceCollection services) { }
}
