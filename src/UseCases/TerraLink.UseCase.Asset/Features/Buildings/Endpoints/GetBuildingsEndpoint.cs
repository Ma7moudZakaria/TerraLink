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
using TerraLink.UseCase.Asset.Features.Buildings.Operations;

namespace TerraLink.UseCase.Asset.Features.Buildings.Endpoints
{
    public sealed partial class GetBuildingsEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/", Handle)
                    .Produces<PagedList<Response>>(StatusCodes.Status200OK)
                    .AddLogging()
                    .WithName("GetAllBuildings")
                    .WithSummary("Get All Buildings")
                    .RequirePermission(Permissions.Assets.View); // Permission filter
        }
        public static async Task<IResult> Handle([AsParameters] Request request, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new GetBuildingsOperation.Request(request), cancellationToken);

            if (result.IsError)
            {
                return result.ToProblem();
            }


            List<Response> items = ObjectMapper.MapList<GetBuildingsOperation.Response, Response>(result.Value.Items);



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
