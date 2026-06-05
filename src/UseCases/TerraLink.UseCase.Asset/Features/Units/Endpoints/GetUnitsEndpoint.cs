using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using LowCodeHub.Permissions;
using TerraLink.UseCase.Asset.Features.Units.Operations;

namespace TerraLink.UseCase.Asset.Features.Units.Endpoints
{
    public sealed partial class GetUnitsEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/", Handle)
            .Produces<PagedList<Response>>(StatusCodes.Status200OK)
            .AddLogging()
            .WithName("GetAllUnits")
            .WithSummary("Get All Units")
            .RequirePermission(Permissions.Assets.View); // Permission filter
        }
        public static async Task<IResult> Handle([AsParameters] Request request, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new GetUnitsOperation.Request(request), cancellationToken);

            if (result.IsError)
            {
                return result.ToProblem();
            }


            List<Response> items = ObjectMapper.MapList<GetUnitsOperation.Response, Response>(result.Value.Items);



            PagedList<Response> response = new()


            {


                Page = result.Value.Page,


                PageSize = result.Value.PageSize,


                TotalCount = result.Value.TotalCount,


                Items = items


            };

            return TypedResults.Ok(response);

        }
    }
}
