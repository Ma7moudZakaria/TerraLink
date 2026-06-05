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
using TerraLink.UseCase.Finance.Features.OutgoingPayments.Operations;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Endpoints;

public sealed partial class DeleteOutgoingPaymentEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id:guid}", Handle)
           .Produces<Response>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status403Forbidden)
           .AddLogging()
           .WithName("DeleteOutgoingPayment")
           .WithSummary("Delete outgoing payment")
           .RequirePermission(Permissions.Finance.Delete);
    }

    public static async Task<IResult> Handle(IOperation dispatcher, [FromRoute] Guid id, CancellationToken ct)
    {
        var result = await dispatcher.ExecuteAsync(new DeleteOutgoingPaymentOperation.Request(id), ct);

        if (result.IsError)
            {
                return result.FirstError.ToBusinessFailure();
            }
            return TypedResults.Ok(Response.Default);
    }
}
