using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using LowCodeHub.Permissions;
using TerraLink.UseCase.Sales.Features.Dashboard.Operations;

namespace TerraLink.UseCase.Sales.Features.Dashboard.Endpoints
{
    public sealed partial class GetDashboardOverviewEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/sales/overview", Handle)
               .Produces<Response>(StatusCodes.Status200OK)
               .WithName("GetSalesDashboardOverview")
               .WithSummary("Get Sales Dashboard Overview")
               .RequirePermission(Permissions.Dashboard.View)
               .AddLogging();
        }

        public static async Task<IResult> Handle(IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new GetSalesDashboardOverviewOperation.Request(), cancellationToken);
            if (result.IsError)
            {
                return result.ToProblem();
            }

            Response response = ObjectMapper.Map<GetSalesDashboardOverviewOperation.Response, Response>(result.Value);

            return TypedResults.Ok(response);
        }
    }
}
