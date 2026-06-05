using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.Lookup.Features.LookupItems.Operations;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Endpoints;

public sealed partial class GetLookupItemsBySetIdEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("/sets/{LookupSetId}/items", Handle)
           .Produces<PagedList<Response>>(StatusCodes.Status200OK)
           .RequireAuthorization()
           .AddLogging()
           .WithName("GetLookupItemsBySetId")
           .WithSummary("Get Lookup Items By Set Id")
           .WithDescription("Retrieve lookup items for a specific lookup set with pagination and filtering");
    }

    public static async Task<IResult> Handle(
        IOperation dispatcher,
        [AsParameters] Request request,
        CancellationToken ct)
    {
        var command = new GetLookupItemsBySetIdOperation.Request(request.LookupSetId,
                                                                 request.Code,
                                                                 request.SearchTerm,
                                                                 request.IsActive,
                                                                 request.PageNumber,
                                                                 request.PageSize);

        var result = await dispatcher.ExecuteAsync(command, ct);


                    List<Response> items = ObjectMapper.MapList<GetLookupItemsBySetIdOperation.Response, Response>(result.Value.Items);



                    PagedList<Response> response = new()


                    {


                        Page = result.Value.Page,


                        PageSize = result.Value.PageSize,


                        TotalCount = result.Value.TotalCount,


                        Items = items


                    };

            return TypedResults.Ok(response);
    }
}
