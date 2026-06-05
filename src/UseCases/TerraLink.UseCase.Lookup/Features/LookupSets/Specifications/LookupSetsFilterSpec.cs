using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.Specifications;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Specifications;

public sealed class LookupSetsFilterSpec(string? code, string? searchTerm, bool? isActive) : ISpecification<LookupSetEntity>
{
    public IQueryable<LookupSetEntity> Where(IQueryable<LookupSetEntity> query)
    {
        if (!string.IsNullOrEmpty(code))
            query = query.Where(x => x.Code.Contains(code));

        if (!string.IsNullOrEmpty(searchTerm))
            query = query.Where(x => x.Code.Contains(searchTerm));

        if (isActive.HasValue)
            query = query.Where(x => x.IsActive == isActive.Value);

        return query;
    }
}
