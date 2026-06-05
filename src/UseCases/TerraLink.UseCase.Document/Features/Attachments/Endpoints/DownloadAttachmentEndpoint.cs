using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.Document.Features.Attachments.Operations;

namespace TerraLink.UseCase.Document.Features.Attachments.Endpoints;

public sealed partial class DownloadAttachmentEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id:guid}", Handle)
           .Produces<Response>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .RequireAuthorization()
           .AddLogging()
           .WithName("DownloadAttachment")
           .WithSummary("Download Attachment")
           .WithDescription("Download an attachment file by its identifier.");
    }

    public static async Task<IResult> Handle(
        IOperation dispatcher,
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await dispatcher.ExecuteAsync(new DownloadAttachmentOperation.Request(id), ct);

        if (result.IsError)
            return result.FirstError.ToNotFound();

                    Response response = ObjectMapper.Map<DownloadAttachmentOperation.Response, Response>(result.Value);

            return TypedResults.File(response.Content, response.ContentType, response.FileName);
    }
}
