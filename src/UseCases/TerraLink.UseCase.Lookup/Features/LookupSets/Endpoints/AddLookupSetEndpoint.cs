using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using LowCodeHub.ObjectMapper;
using LowCodeHub.Permissions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using TerraLink.UseCase.Lookup.Features.LookupSets.Operations;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Endpoints;

public sealed partial class AddLookupSetEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("/sets", Handle)
           .Accepts<Request>("application/json")
           .Produces<Response>(StatusCodes.Status201Created)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status409Conflict)
           .AddValidator<Request>()
           .AddLogging()
           .WithName("CreateLookupSet")
           .WithSummary("Create Lookup Set")
           .WithDescription("Create a new lookup set")
           .RequirePermission(Permissions.Lookups.Create);
    }

    public static async Task<IResult> Handle(
        IOperation dispatcher,
        Request request,
        CancellationToken ct)
    {
        AddLookupSetOperation.Request command = ObjectMapper.Map<Request, AddLookupSetOperation.Request>(request);
        var result = await dispatcher.ExecuteAsync(command, ct);

        if (result.IsError)
        {
            return result.FirstError.ToConflict();
        }

        Response response = ObjectMapper.Map<AddLookupSetOperation.Response, Response>(result.Value);

        return TypedResults.Created($"/api/lookups/sets/{response.Id}", response);
    }
}
