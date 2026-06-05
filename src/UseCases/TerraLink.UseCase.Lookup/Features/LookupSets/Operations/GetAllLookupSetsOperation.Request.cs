using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Models;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Operations;

public sealed partial class GetAllLookupSetsOperation
{
    public sealed record Request(string? Code, string? SearchTerm, bool? IsActive, int Page, int PageSize)
    : IOperationRequest<PagedList<Response>>;
}
