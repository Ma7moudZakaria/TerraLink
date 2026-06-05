using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.Report.Features.Reports.Operations;

namespace TerraLink.UseCase.Report.Features.Reports.Endpoints;

public sealed partial class ExportExcelEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("/excel", Handle)
           .Produces<Response>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .RequireAuthorization()
           .AddLogging()
           .WithName("ExportExcel")
           .WithSummary("Export Excel")
           .WithDescription("Export an Excel report whose columns are described by a lookup set.");
    }

    public static async Task<IResult> Handle(
        IOperation dispatcher,
        [AsParameters] Request request,
        CancellationToken ct)
    {
        var result = await dispatcher.ExecuteAsync(new ExportExcelOperation.Request(request.LookupSetId), ct);

        if (result.IsError)
            return result.FirstError.ToUnprocessableEntity();

                    Response response = ObjectMapper.Map<ExportExcelOperation.Response, Response>(result.Value);

            return TypedResults.File(response.File, response.ContentType, response.FileName);
    }
}
