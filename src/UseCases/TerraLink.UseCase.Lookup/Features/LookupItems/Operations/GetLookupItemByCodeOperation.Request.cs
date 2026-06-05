using LowCodeHub.MinimalEndpoints.Abstractions;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Operations;

public sealed partial class GetLookupItemByCodeOperation
{
    public sealed record Request(string SetCode, string ItemCode) : IOperationRequest<Response>;
}
