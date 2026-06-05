using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.UseCase.Lookup.Features.LookupSets.Specifications;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Operations;

public sealed partial class GetAllLookupSetsOperation(IRepository<LookupSetEntity> lookupSets, IMapper mapper)
    : IOperationHandler<GetAllLookupSetsOperation.Request, PagedList<GetAllLookupSetsOperation.Response>>
{
    public async Task<ErrorOr<PagedList<GetAllLookupSetsOperation.Response>>> HandleAsync(
        GetAllLookupSetsOperation.Request request,
        CancellationToken ct = default)
    {
        PagedList<LookupSetEntity> result = await lookupSets.PagedAsync(
            new LookupSetsListSpec(request.Code, request.SearchTerm, request.IsActive),
            request.Page,
            request.PageSize,
            ct);

        return new PagedList<GetAllLookupSetsOperation.Response>
        {
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            Items = result.Items.Select(mapper.Map<LookupSetEntity, GetAllLookupSetsOperation.Response>).ToList()
        };
    }
}
