using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Models;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Operations;

public sealed partial class GetLookupItemsBySetIdOperation
{
    public sealed record Request(
    Guid LookupSetId,
    string? Code,
    string? SearchTerm,
    bool? IsActive,
    int Page,
    int PageSize) : IOperationRequest<PagedList<Response>>;
}
