using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.Permissions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using TerraLink.UseCase.Finance.Features.IncomingPayments.Operations;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Endpoints;

public sealed partial class UpdateIncomingPaymentEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPut("/{id:guid}", Handle)
           .Produces<Response>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status403Forbidden)
           .AddLogging()
           .WithName("UpdateIncomingPayment")
           .WithSummary("Update incoming payment")
           .RequirePermission(Permissions.Finance.Edit);
    }

    public static async Task<IResult> Handle(
        IOperation dispatcher,
        [FromRoute] Guid id,
        Request request,
        CancellationToken ct)
    {
        var command = new UpdateIncomingPaymentOperation.Request(
            id,
            request.ContractId,
            request.ContractInstallmentId,
            request.ClientId,
            request.SourceType,
            request.TransactionTypeId,
            request.Amount,
            request.PaymentMethodId,
            request.PaymentDate,
            request.Notes,
            request.Attachments);

        var result = await dispatcher.ExecuteAsync(command, ct);

        if (result.IsError)
        {
            return result.FirstError.ToBusinessFailure();
        }
        return TypedResults.Ok(Response.Default);
    }
}
