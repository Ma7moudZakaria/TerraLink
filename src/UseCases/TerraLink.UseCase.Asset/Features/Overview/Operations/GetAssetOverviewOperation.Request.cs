using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.UseCase.Asset.Features.Overview.Endpoints;

namespace TerraLink.UseCase.Asset.Features.Overview.Operations;

public sealed partial class GetAssetOverviewOperation
{
    public sealed record Request : IOperationRequest<Response>;
}
