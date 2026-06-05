namespace TerraLink.UseCase.Document.Features.Attachments.Operations;

public sealed partial class DownloadAttachmentOperation
{
    public sealed record Response(byte[] Content, string ContentType, string FileName);
}
