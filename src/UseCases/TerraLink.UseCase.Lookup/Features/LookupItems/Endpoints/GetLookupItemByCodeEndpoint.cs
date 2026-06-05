using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.Lookup.Features.LookupItems.Operations;
using TerraLink.UseCase.Lookup.Features.LookupItems.Specifications;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Endpoints;

public sealed partial class GetLookupItemByCodeEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("/sets/by-code/{SetCode}/items/by-code/{ItemCode}", Handle)
           .Produces<Response>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .RequireAuthorization()
           .AddLogging()
           .WithName("GetLookupItemByCode")
           .WithSummary("Get Lookup Item By Code")
           .WithDescription("Retrieve a lookup item by both lookup set code and item code");
    }

    public static async Task<IResult> Handle(
        IOperation dispatcher,
        [AsParameters] Request request,
        CancellationToken ct)
    {
        var result = await dispatcher.ExecuteAsync(
            new GetLookupItemByCodeOperation.Request(request.SetCode, request.ItemCode), ct);

        if (result.IsError)
            {
                return result.FirstError.ToNotFound();
            }

            Response response = ObjectMapper.Map<GetLookupItemByCodeOperation.Response, Response>(result.Value);

            return TypedResults.Ok(response);
    }
}
