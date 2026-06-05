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

public sealed partial class UploadAttachmentEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("/{attachmentTypeId:guid}", Handle)
           .Accepts<IFormFile>(false, "multipart/form-data")
           .Produces<Response>(StatusCodes.Status201Created)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .DisableAntiforgery()
           .RequireAuthorization()
           .AddLogging()
           .WithName("UploadAttachment")
           .WithSummary("Upload Attachment")
           .WithDescription("Upload an attachment file. Max size 5 MB. Allowed extensions: pdf/doc/docx/jpg/png/m4a/mp4.");
    }

    public static async Task<IResult> Handle(
        IOperation dispatcher,
        [FromRoute] Guid attachmentTypeId,
        [FromForm(Name = "file")] IFormFile file,
        CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return ErrorOr.Error.Validation("Attachment.Empty", "File is empty.").ToBadRequest();

        await using var stream = file.OpenReadStream();
        var command = new UploadAttachmentOperation.Request(
            attachmentTypeId,
            file.FileName,
            Path.GetExtension(file.FileName),
            file.Length,
            stream);

        var result = await dispatcher.ExecuteAsync(command, ct);

        if (result.IsError)
        {
            return result.FirstError.ToUnprocessableEntity();
        }

        Response response = ObjectMapper.Map<UploadAttachmentOperation.Response, Response>(result.Value);

        return TypedResults.Created($"/api/documents/{response.Id}", response);
    }
}
