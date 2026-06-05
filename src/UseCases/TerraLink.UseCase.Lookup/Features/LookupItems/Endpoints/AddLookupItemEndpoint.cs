using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using LowCodeHub.ObjectMapper;
using LowCodeHub.Permissions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using TerraLink.UseCase.Lookup.Features.LookupItems.Operations;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Endpoints;

public sealed partial class AddLookupItemEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("/items", Handle)
           .Accepts<Request>("application/json")
           .Produces<Response>(StatusCodes.Status201Created)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status409Conflict)
           .AddValidator<Request>()
           .AddLogging()
           .WithName("CreateLookupItem")
           .WithSummary("Create Lookup Item")
           .WithDescription("Create a new lookup item for a specific lookup set")
           .RequirePermission(Permissions.Lookups.Create);
    }

    public static async Task<IResult> Handle(
        IOperation dispatcher,
        Request request,
        CancellationToken ct)
    {
        AddLookupItemOperation.Request command = ObjectMapper.Map<Request, AddLookupItemOperation.Request>(request);
        var result = await dispatcher.ExecuteAsync(command, ct);

        if (result.IsError)
        {
            return result.FirstError.ToConflict();
        }

        Response response = ObjectMapper.Map<AddLookupItemOperation.Response, Response>(result.Value);

        return TypedResults.Created($"/api/lookups/items/{response.Id}", response);
    }
}
