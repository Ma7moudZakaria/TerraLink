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
using TerraLink.UseCase.Finance.Features.Installments.Operations;

namespace TerraLink.UseCase.Finance.Features.Installments.Endpoints;

public sealed partial class CreateInstallmentEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("/", Handle)
           .Produces<Response>(StatusCodes.Status201Created)
           .ProducesProblem(StatusCodes.Status403Forbidden)
           .AddLogging()
           .WithName("CreateInstallment")
           .WithSummary("Create installment")
           .RequirePermission(Permissions.Finance.Create);
    }

    public static async Task<IResult> Handle(IOperation dispatcher, Request request, CancellationToken ct)
    {
        CreateInstallmentOperation.Request command = ObjectMapper.Map<Request, CreateInstallmentOperation.Request>(request);
        var result = await dispatcher.ExecuteAsync(command, ct);
        if (result.IsError)
            {
                return result.FirstError.ToBusinessFailure();
            }
            return TypedResults.Ok(Response.Default);
    }
}
