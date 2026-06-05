using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using LowCodeHub.ObjectMapper;
using LowCodeHub.Permissions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using TerraLink.UseCase.Finance.Features.OutgoingPayments.Operations;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Endpoints;

public sealed partial class CreateOutgoingPaymentEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("/", Handle)
           .Produces<Response>(StatusCodes.Status201Created)
           .ProducesProblem(StatusCodes.Status403Forbidden)
           .AddValidator<Request>()
           .AddLogging()
           .WithName("CreateOutgoingPayment")
           .WithSummary("Create outgoing payment")
           .RequirePermission(Permissions.Finance.Create);
    }

    public static async Task<IResult> Handle(
        IOperation dispatcher,
        Request request,
        CancellationToken ct)
    {
        CreateOutgoingPaymentOperation.Request command = ObjectMapper.Map<Request, CreateOutgoingPaymentOperation.Request>(request);
        var result = await dispatcher.ExecuteAsync(command, ct);

        if (result.IsError)
            {
                return result.FirstError.ToBusinessFailure();
            }
            return TypedResults.Ok(Response.Default);
    }
}
