using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using LowCodeHub.Permissions;
using TerraLink.UseCase.IdentityShield.Features.Users.Operations;

namespace TerraLink.UseCase.IdentityShield.Features.Users.Endpoints
{
    public sealed partial class GetUsersEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/", Handle)
               .Produces<PagedList<Response>>(StatusCodes.Status200OK)
               .ProducesProblem(StatusCodes.Status403Forbidden)
               .WithName("GetUsers")
               .WithSummary("Get Users")
               .WithDescription("Retrieve system users with filtering and search support")
               .RequirePermission(Permissions.Identity.View)
               .AddLogging();
        }

        public static async Task<IResult> Handle([AsParameters] Request request, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new GetUsersOperation.Request(request), cancellationToken);

            if (result.IsError)
            {
                return result.ToProblem();
            }


            List<Response> items = ObjectMapper.MapList<GetUsersOperation.Response, Response>(result.Value.Items);



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
