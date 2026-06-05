using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.Specifications;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Specifications;

public sealed class LookupItemsFilterSpec(Guid lookupSetId, string? code, string? searchTerm, bool? isActive) : ISpecification<LookupItemEntity>
{
    public IQueryable<LookupItemEntity> Where(IQueryable<LookupItemEntity> query)
    {
        query = query.Where(x => x.LookupSetId == lookupSetId);

        if (!string.IsNullOrEmpty(code))
            query = query.Where(x => x.Code.Contains(code));

        if (!string.IsNullOrEmpty(searchTerm))
            query = query.Where(x => x.Code.Contains(searchTerm));

        if (isActive.HasValue)
            query = query.Where(x => x.IsActive == isActive.Value);

        return query;
    }
}
