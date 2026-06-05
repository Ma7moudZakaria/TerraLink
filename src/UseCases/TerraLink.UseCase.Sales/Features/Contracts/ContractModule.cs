using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.Sales.Features.Contracts.Endpoints;

namespace TerraLink.UseCase.Sales.Features.Contracts
{
    public sealed class ContractModule : IModule
    {
        public static void AddRoutes(IEndpointRouteBuilder app)
        {
            RouteGroupBuilder group = app.MapGroup("/api/contracts")
                                         .WithTags("Contracts")
                                         .WithGroupName("sales-module")
                                         .RequireAuthorization(); // All endpoints require authentication

            group.MapEndpoint<GetAllContractsEndpoint>()
                 .MapEndpoint<GetContractDetailsEndpoint>()
                 .MapEndpoint<GetContractOverviewEndpoint>()
                 .MapEndpoint<CreateContractEndpoint>()
                 .MapEndpoint<UpdateContractEndpoint>()
                 .MapEndpoint<DeleteContractEndpoint>()
                 .MapEndpoint<GetContractsDropdownEndpoint>();
        }
    }
}
