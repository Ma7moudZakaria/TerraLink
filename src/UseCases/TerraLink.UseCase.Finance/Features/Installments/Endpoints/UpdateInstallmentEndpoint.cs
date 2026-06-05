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
using TerraLink.UseCase.Finance.Features.Installments.Operations;

namespace TerraLink.UseCase.Finance.Features.Installments.Endpoints;

public sealed partial class UpdateInstallmentEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPut("/{id:guid}", Handle)
           .Produces<Response>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status403Forbidden)
           .AddLogging()
           .WithName("UpdateInstallment")
           .WithSummary("Update installment")
           .RequirePermission(Permissions.Finance.Edit);
    }

    public static async Task<IResult> Handle(IOperation dispatcher, [FromRoute] Guid id, Request request, CancellationToken ct)
    {
        var command = new UpdateInstallmentOperation.Request(id, request.ContractId, request.Description, request.DueDate, request.Amount, request.AmountText);
        var result = await dispatcher.ExecuteAsync(command, ct);
        if (result.IsError)
            {
                return result.FirstError.ToBusinessFailure();
            }
            return TypedResults.Ok(Response.Default);
    }
}
