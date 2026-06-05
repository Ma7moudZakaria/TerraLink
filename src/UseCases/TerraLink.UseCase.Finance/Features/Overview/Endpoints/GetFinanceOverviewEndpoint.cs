using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using LowCodeHub.Permissions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using TerraLink.UseCase.Finance.Features.Overview.Operations;

namespace TerraLink.UseCase.Finance.Features.Overview.Endpoints;

public sealed partial class GetFinanceOverviewEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("/overview", Handle)
           .Produces<Response>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status403Forbidden)
           .AddLogging()
           .WithName("GetFinanceOverview")
           .WithSummary("Get finance overview")
           .RequirePermission(Permissions.Finance.View);
    }

    public static async Task<IResult> Handle(IOperation dispatcher, CancellationToken ct)
    {
        var result = await dispatcher.ExecuteAsync(new GetFinanceOverviewOperation.Request(), ct);
                    Response response = ObjectMapper.Map<GetFinanceOverviewOperation.Response, Response>(result.Value);

            return TypedResults.Ok(response);
    }
}
