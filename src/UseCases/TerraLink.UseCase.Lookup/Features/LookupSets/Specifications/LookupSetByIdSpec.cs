using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.Specifications;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Specifications;

public sealed class LookupSetByIdSpec(Guid id) : ISpecification<LookupSetEntity>
{
    public IQueryable<LookupSetEntity> Where(IQueryable<LookupSetEntity> query)
        => query.Where(x => x.Id == id);
}
