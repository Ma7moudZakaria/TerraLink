using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.Specifications;
using Microsoft.EntityFrameworkCore;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Specifications;

public sealed class LookupItemDetailsSpec(Guid lookupSetId, string itemCode) : ISpecification<LookupItemEntity>
{
    public IQueryable<LookupItemEntity> Where(IQueryable<LookupItemEntity> query)
    {
        return query
            .Include(entity => entity.LookupSet)
            .Where(entity => entity.LookupSetId == lookupSetId && entity.Code == itemCode);
    }
}

public sealed class LookupItemsListSpec(Guid lookupSetId, string? code, string? searchTerm, bool? isActive)
    : ISpecification<LookupItemEntity>
{
    public IQueryable<LookupItemEntity> Where(IQueryable<LookupItemEntity> query)
    {
        query = query.Where(entity => entity.LookupSetId == lookupSetId);

        if (!string.IsNullOrWhiteSpace(code))
        {
            query = query.Where(entity => entity.Code.Contains(code));
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(entity => entity.Code.Contains(searchTerm));
        }

        if (isActive.HasValue)
        {
            query = query.Where(entity => entity.IsActive == isActive.Value);
        }

        return query
            .OrderBy(entity => entity.SortOrder)
            .ThenBy(entity => entity.Code);
    }
}
