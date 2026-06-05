using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.Specifications;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Specifications;

public sealed class LookupItemBySetIdAndCodeSpec(Guid lookupSetId, string itemCode) : ISpecification<LookupItemEntity>
{
    public IQueryable<LookupItemEntity> Where(IQueryable<LookupItemEntity> query)
        => query.Where(x => x.LookupSetId == lookupSetId && x.Code == itemCode);
}
