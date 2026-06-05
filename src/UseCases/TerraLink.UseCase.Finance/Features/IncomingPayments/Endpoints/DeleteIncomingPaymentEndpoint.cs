using ErrorOr;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Abstractions;
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

public sealed partial class DeleteIncomingPaymentEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id:guid}", Handle)
           .Produces<Response>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status403Forbidden)
           .AddLogging()
           .WithName("DeleteIncomingPayment")
           .WithSummary("Delete incoming payment")
           .RequirePermission(Permissions.Finance.Delete);
    }

    public static async Task<IResult> Handle(IOperation dispatcher, [FromRoute] Guid id, CancellationToken ct)
    {
        var result = await dispatcher.ExecuteAsync(new DeleteIncomingPaymentOperation.Request(id), ct);

        if (result.IsError)
            {
                return result.FirstError.ToBusinessFailure();
            }
            return TypedResults.Ok(Response.Default);
    }
}
