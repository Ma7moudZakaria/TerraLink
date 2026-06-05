namespace TerraLink.UseCase.Document.Features.Attachments.Operations;

public sealed partial class UploadAttachmentOperation
{
    public sealed class Response
    {
        public Guid Id { get; set; }
    }
}