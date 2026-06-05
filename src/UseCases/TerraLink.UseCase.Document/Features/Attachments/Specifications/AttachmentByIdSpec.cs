using LowCodeHub.QueryableExtensions.Specifications;
using TerraLink.Domain.Entities;

namespace TerraLink.UseCase.Document.Features.Attachments.Specifications;

public sealed class AttachmentByIdSpec(Guid id) : ISpecification<AttachmentItemEntity>
{
    public IQueryable<AttachmentItemEntity> Where(IQueryable<AttachmentItemEntity> query)
        => query.Where(x => x.Id == id);
}
