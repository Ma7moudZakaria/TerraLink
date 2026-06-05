using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.Specifications;
using Microsoft.EntityFrameworkCore;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Specifications;

public sealed class LookupSetDetailsSpec(string code) : ISpecification<LookupSetEntity>
{
    public IQueryable<LookupSetEntity> Where(IQueryable<LookupSetEntity> query)
    {
        return query
            .Include(entity => entity.LookupItems)
            .Where(entity => entity.Code == code);
    }
}

public sealed class LookupSetsListSpec(string? code, string? searchTerm, bool? isActive) : ISpecification<LookupSetEntity>
{
    public IQueryable<LookupSetEntity> Where(IQueryable<LookupSetEntity> query)
    {
        query = query.Include(entity => entity.LookupItems);

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

        return query.OrderBy(entity => entity.Code);
    }
}
