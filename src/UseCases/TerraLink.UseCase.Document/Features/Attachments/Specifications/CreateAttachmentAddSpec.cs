using LowCodeHub.QueryableExtensions.Specifications;
using TerraLink.Domain.Entities;
using TerraLink.Domain.ValueObjects;

namespace TerraLink.UseCase.Document.Features.Attachments.Specifications;

public sealed class CreateAttachmentAddSpec(
    Guid id,
    string name,
    AttachmentExtension extension,
    string mimeType,
    Guid attachmentTypeId) : IAddSpecification<AttachmentItemEntity>
{
    public AttachmentItemEntity Add() => new()
    {
        Id = id,
        Name = name,
        Extension = extension,
        MimeType = mimeType,
        AttachmentTypeId = attachmentTypeId
    };
}
