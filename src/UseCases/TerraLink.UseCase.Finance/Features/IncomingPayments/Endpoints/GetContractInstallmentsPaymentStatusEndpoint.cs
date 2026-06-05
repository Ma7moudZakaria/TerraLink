using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using LowCodeHub.Permissions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using TerraLink.UseCase.Finance.Features.IncomingPayments.Operations;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Endpoints;

public sealed partial class GetContractInstallmentsPaymentStatusEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("/contracts/{contractId:guid}/installments", Handle)
           .Produces<List<Response>>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status403Forbidden)
           .AddLogging()
           .WithName("GetContractInstallmentsPaymentStatus")
           .WithSummary("Get installments payment status by contract id")
           .RequirePermission(Permissions.Finance.View);
    }

    public static async Task<IResult> Handle(IOperation dispatcher, [FromRoute] Guid contractId, CancellationToken ct)
    {
        var result = await dispatcher.ExecuteAsync(new GetContractInstallmentsPaymentStatusOperation.Request(contractId), ct);
                    List<Response> response = ObjectMapper.MapList<GetContractInstallmentsPaymentStatusOperation.Response, Response>(result.Value);

            return TypedResults.Ok(response);
    }
}
