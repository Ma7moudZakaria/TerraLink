using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.Sales.Features.Clients.Endpoints;

namespace TerraLink.UseCase.Sales.Features.Clients
{
    public sealed class ClientModule : IModule
    {
        public static void AddRoutes(IEndpointRouteBuilder app)
        {
            RouteGroupBuilder group = app.MapGroup("/api/clients")
                                         .WithTags("Clients")
                                         .WithGroupName("sales-module")
                                         .RequireAuthorization();

            group.MapEndpoint<GetAllClientsEndpoint>()
                 .MapEndpoint<GetClientDetailsEndpoint>()
                 .MapEndpoint<GetClientOverviewEndpoint>()
                 .MapEndpoint<CreateClientEndpoint>()
                 .MapEndpoint<UpdateClientEndpoint>()
                 .MapEndpoint<DeleteClientEndpoint>()
                 .MapEndpoint<GetClientsDropdownEndpoint>();
        }
    }
}
