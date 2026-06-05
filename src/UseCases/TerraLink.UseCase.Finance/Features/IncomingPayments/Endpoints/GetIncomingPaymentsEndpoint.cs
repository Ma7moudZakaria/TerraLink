using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using LowCodeHub.Permissions;
using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using TerraLink.UseCase.Finance.Features.IncomingPayments.Operations;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Endpoints;

public sealed partial class GetIncomingPaymentsEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("/", Handle)
           .Produces<PagedList<Response>>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status403Forbidden)
           .AddLogging()
           .WithName("GetIncomingPayments")
           .WithSummary("Get incoming payments")
           .RequirePermission(Permissions.Finance.View);
    }

    public static async Task<IResult> Handle(
        IOperation dispatcher,
        [AsParameters] Request request,
        CancellationToken ct)
    {
        var command = new GetIncomingPaymentsOperation.Request(
            request.Code, request.ContractCode, request.ClientName, request.UnitCode,
            request.Amount, request.PaymentDate, request.PageNumber, request.PageSize);

        var result = await dispatcher.ExecuteAsync(command, ct);

                    List<Response> items = ObjectMapper.MapList<GetIncomingPaymentsOperation.Response, Response>(result.Value.Items);


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
