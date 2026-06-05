using LowCodeHub.MinimalEndpoints.Abstractions;

namespace TerraLink.UseCase.Document.Features.Attachments.Operations;

public sealed partial class UploadAttachmentOperation
{
    public sealed record Request(
    Guid AttachmentTypeId,
    string FileName,
    string Extension,
    long Size,
    Stream Content) : IOperationRequest<Response>;
}
