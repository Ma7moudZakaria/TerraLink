using LowCodeHub.MinimalEndpoints.Abstractions;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Operations;

public sealed partial class GetLookupSetByCodeOperation
{
    public sealed record Request(string Code) : IOperationRequest<Response>;
}
