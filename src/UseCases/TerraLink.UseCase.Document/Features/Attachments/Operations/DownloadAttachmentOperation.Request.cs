using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.UseCase.Document.Features.Attachments.Operations;

namespace TerraLink.UseCase.Document.Features.Attachments.Operations;

public sealed partial class DownloadAttachmentOperation
{
    public sealed record Request(Guid Id) : IOperationRequest<DownloadAttachmentOperation.Response>;
}
