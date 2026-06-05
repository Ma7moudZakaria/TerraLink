using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.UseCase.Lookup.Features.LookupItems.Specifications;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Operations;

public sealed partial class GetLookupItemsBySetIdOperation(IRepository<LookupItemEntity> lookupItems, IMapper mapper)
    : IOperationHandler<GetLookupItemsBySetIdOperation.Request, PagedList<GetLookupItemsBySetIdOperation.Response>>
{
    public async Task<ErrorOr<PagedList<GetLookupItemsBySetIdOperation.Response>>> HandleAsync(
        GetLookupItemsBySetIdOperation.Request request,
        CancellationToken ct = default)
    {
        PagedList<LookupItemEntity> result = await lookupItems.PagedAsync(
            new LookupItemsListSpec(request.LookupSetId, request.Code, request.SearchTerm, request.IsActive),
            request.Page,
            request.PageSize,
            ct);

        return new PagedList<GetLookupItemsBySetIdOperation.Response>
        {
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            Items = result.Items
                .Select(mapper.Map<LookupItemEntity, GetLookupItemsBySetIdOperation.Response>)
                .ToList()
        };
    }
}
