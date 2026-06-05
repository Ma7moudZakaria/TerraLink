using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using Microsoft.AspNetCore.Hosting;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Document.Features.Attachments.Specifications;

namespace TerraLink.UseCase.Document.Features.Attachments.Operations;

public sealed partial class DownloadAttachmentOperation(IRepository<AttachmentItemEntity> attachments, IWebHostEnvironment env)
    : IOperationHandler<DownloadAttachmentOperation.Request, DownloadAttachmentOperation.Response>
{
    public async Task<ErrorOr<DownloadAttachmentOperation.Response>> HandleAsync(
        DownloadAttachmentOperation.Request request, CancellationToken ct = default)
    {
        AttachmentItemEntity? attachment = await attachments.GetAsync(
            new AttachmentByIdSpec(request.Id),
            ct);

        if (attachment is null)
            return Error.NotFound("Attachment.NotFound", $"Attachment {request.Id} not found.");

        var directory = Path.Combine(env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "attachments");
        var filePath = Path.Combine(directory, $"{attachment.Id}{attachment.Extension.Value}");

        if (!File.Exists(filePath))
            return Error.NotFound("Attachment.FileMissing", "File not found on server.");

        var content = await File.ReadAllBytesAsync(filePath, ct);
        return new DownloadAttachmentOperation.Response(content, attachment.MimeType, attachment.Name + attachment.Extension.Value);
    }
}
