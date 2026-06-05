namespace TerraLink.UseCase.Document.Features.Attachments.Endpoints;

public sealed partial class DownloadAttachmentEndpoint
{
    public sealed record Response(byte[] Content, string ContentType, string FileName);
}
