using ErrorOr;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using LowCodeHub.Permissions;
using TerraLink.UseCase.Sales.Features.Clients.Operations;

namespace TerraLink.UseCase.Sales.Features.Clients.Endpoints
{
    public sealed partial class GetAllClientsEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/", Handle)
               .Produces<PagedList<Response>>(StatusCodes.Status200OK)
               .AddLogging()
               .WithName("GetAllClients")
               .WithSummary("Get All Clients")
               .RequirePermission(Permissions.Clients.View); // Permission filter
        }

        public static async Task<IResult> Handle([AsParameters] Request request,
                                                 IOperation dispatcher,
                                                 CancellationToken cancellationToken)
        {
            var getResult = await dispatcher.ExecuteAsync(new GetClientsOperation.Request(request), cancellationToken);

            if (getResult.IsError)
            {
                return getResult.FirstError.ToUnprocessableEntity();
            }


                        List<Response> items = ObjectMapper.MapList<GetClientsOperation.Response, Response>(getResult.Value.Items);



                        PagedList<Response> response = new()


                        {


                            Page = getResult.Value.Page,


                            PageSize = getResult.Value.PageSize,


                            TotalCount = getResult.Value.TotalCount,


                            Items = items


                        };

            return TypedResults.Ok(response);
        }
    }
}
