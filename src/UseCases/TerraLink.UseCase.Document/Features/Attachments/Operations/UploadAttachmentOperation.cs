using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Hosting;
using TerraLink.Domain.Entities;
using TerraLink.Domain.ValueObjects;
using TerraLink.UseCase.Document.Features.Attachments.Specifications;

namespace TerraLink.UseCase.Document.Features.Attachments.Operations;

public sealed partial class UploadAttachmentOperation(IRepository<AttachmentItemEntity> attachments, IWebHostEnvironment env)
    : IOperationHandler<UploadAttachmentOperation.Request, UploadAttachmentOperation.Response>
{
    private const long MaxFileSize = 5 * 1024 * 1024;

    public async Task<ErrorOr<UploadAttachmentOperation.Response>> HandleAsync(UploadAttachmentOperation.Request request, CancellationToken ct = default)
    {
        if (request.Size <= 0)
            return Error.Validation("Attachment.Empty", "File is empty.");

        if (request.Size > MaxFileSize)
            return Error.Validation("Attachment.TooLarge", "File size exceeds the maximum allowed size of 5 MB.");

        var extResult = AttachmentExtension.Create(request.Extension);
        if (extResult.IsError)
            return extResult.Errors;

        if (!new FileExtensionContentTypeProvider().TryGetContentType(request.FileName, out string? contentType))
            contentType = "application/octet-stream";

        var id = Guid.CreateVersion7();
        attachments.Add(new CreateAttachmentAddSpec(id, Path.GetFileNameWithoutExtension(request.FileName), extResult.Value, contentType, request.AttachmentTypeId));

        var directory = Path.Combine(env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "attachments");
        Directory.CreateDirectory(directory);

        var filePath = Path.Combine(directory, $"{id}{request.Extension}");
        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await request.Content.CopyToAsync(fileStream, ct);
        }

        await attachments.SaveChangesAsync(ct);
        return new UploadAttachmentOperation.Response { Id = id };
    }
}
