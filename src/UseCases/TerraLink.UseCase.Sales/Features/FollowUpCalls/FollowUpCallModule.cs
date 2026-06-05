using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.Sales.Features.FollowUpCalls.Endpoints;

namespace TerraLink.UseCase.Sales.Features.FollowUpCalls
{
    public sealed class FollowUpCallModule : IModule
    {
        public static void AddRoutes(IEndpointRouteBuilder app)
        {
            RouteGroupBuilder group = app.MapGroup("/api/clients/{clientId}/follow-up-calls")
                .WithTags("FollowUpCalls")
                .WithGroupName("sales-module")
                .RequireAuthorization();

            group.MapEndpoint<GetAllFollowUpCallsEndpoint>()
                .MapEndpoint<GetFollowUpCallDetailsEndpoint>()
                .MapEndpoint<CreateFollowUpCallEndpoint>()
                .MapEndpoint<DeleteFollowUpCallEndpoint>()
                .MapEndpoint<UpdateFollowUpCallEndpoint>();
        }
    }
}
