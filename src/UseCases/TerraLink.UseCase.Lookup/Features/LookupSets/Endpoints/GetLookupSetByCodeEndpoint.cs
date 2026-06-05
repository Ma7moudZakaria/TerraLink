using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.Lookup.Features.LookupSets.Operations;
using TerraLink.UseCase.Lookup.Features.LookupSets.Specifications;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Endpoints;

public sealed partial class GetLookupSetByCodeEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("/sets/by-code/{Code}", Handle)
           .Produces<Response>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .RequireAuthorization()
           .AddLogging()
           .WithName("GetLookupSetByCode")
           .WithSummary("Get Lookup Set By Code")
           .WithDescription("Retrieve a lookup set by its code");
    }

    public static async Task<IResult> Handle(
        IOperation dispatcher,
        [AsParameters] Request request,
        CancellationToken ct)
    {
        var result = await dispatcher.ExecuteAsync(new GetLookupSetByCodeOperation.Request(request.Code), ct);

        if (result.IsError)
            {
                return result.FirstError.ToNotFound();
            }

            Response response = ObjectMapper.Map<GetLookupSetByCodeOperation.Response, Response>(result.Value);

            return TypedResults.Ok(response);
    }
}
