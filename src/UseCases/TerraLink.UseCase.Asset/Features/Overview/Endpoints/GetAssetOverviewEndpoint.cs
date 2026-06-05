using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using LowCodeHub.Permissions;
using TerraLink.UseCase.Asset.Features.Overview.Operations;

namespace TerraLink.UseCase.Asset.Features.Overview.Endpoints
{
    public sealed partial class GetAssetOverviewEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/overview", Handle)
               .Produces<Response>(StatusCodes.Status200OK)
               .WithName("GetAssetsOverview")
               .WithSummary("Get Assets Overview")
               .RequirePermission(Permissions.Assets.View)
               .AddLogging();
        }

        public static async Task<IResult> Handle(IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new GetAssetOverviewOperation.Request(), cancellationToken);
            if (result.IsError)
            {
                return result.ToProblem();
            }

            Response response = ObjectMapper.Map<GetAssetOverviewOperation.Response, Response>(result.Value);

            return TypedResults.Ok(response);
        }
    }
}
