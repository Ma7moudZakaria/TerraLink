using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;

namespace TerraLink.UseCase.Finance.Features.Overview.Operations;

public sealed partial class GetFinanceOverviewOperation
{
    public sealed record Request : IOperationRequest<Response>;
}
