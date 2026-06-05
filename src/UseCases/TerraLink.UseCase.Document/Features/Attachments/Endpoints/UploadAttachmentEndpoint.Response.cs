namespace TerraLink.UseCase.Document.Features.Attachments.Endpoints;

public sealed partial class UploadAttachmentEndpoint
{
    public sealed class Response
    {
        public Guid Id { get; set; }
    }
}