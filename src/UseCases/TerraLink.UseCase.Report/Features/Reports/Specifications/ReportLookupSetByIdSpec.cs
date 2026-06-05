using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.Specifications;
using Microsoft.EntityFrameworkCore;

namespace TerraLink.UseCase.Report.Features.Reports.Specifications;

public sealed class ReportLookupSetByIdSpec(Guid id) : ISpecification<LookupSetEntity>
{
    public IQueryable<LookupSetEntity> Where(IQueryable<LookupSetEntity> query)
        => query
            .Include(x => x.LookupItems)
            .Where(x => x.Id == id);
}
