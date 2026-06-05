using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Operations;

public sealed partial class AddLookupSetOperation
{
    public sealed record Request(string Code, Localized? Descriptions) : IOperationRequest<Response>;
}
