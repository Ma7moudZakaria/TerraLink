using LowCodeHub.MinimalEndpoints.Abstractions;

namespace TerraLink.UseCase.Dashboard.Features.Overview.Operations;

public sealed partial class GetUserDashboardOverviewOperation
{
    public sealed record Request : IOperationRequest<Response>;
}
