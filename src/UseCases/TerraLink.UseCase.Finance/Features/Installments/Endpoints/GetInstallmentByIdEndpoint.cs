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
using TerraLink.UseCase.Finance.Features.Installments.Operations;

namespace TerraLink.UseCase.Finance.Features.Installments.Endpoints;

public sealed partial class GetInstallmentByIdEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id:guid}", Handle)
           .Produces<Response>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .ProducesProblem(StatusCodes.Status403Forbidden)
           .AddLogging()
           .WithName("GetInstallmentById")
           .WithSummary("Get installment by id")
           .RequirePermission(Permissions.Finance.View);
    }

    public static async Task<IResult> Handle(IOperation dispatcher, [FromRoute] Guid id, CancellationToken ct)
    {
        var result = await dispatcher.ExecuteAsync(new GetInstallmentByIdOperation.Request(id), ct);
        if (result.IsError)
            {
                return result.FirstError.ToNotFound();
            }

            Response response = ObjectMapper.Map<GetInstallmentByIdOperation.Response, Response>(result.Value);

            return TypedResults.Ok(response);
    }
}
