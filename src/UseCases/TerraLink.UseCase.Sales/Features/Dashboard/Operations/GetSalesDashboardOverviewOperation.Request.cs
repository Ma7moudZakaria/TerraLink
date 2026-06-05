using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.UseCase.Sales.Features.Dashboard.Endpoints;

namespace TerraLink.UseCase.Sales.Features.Dashboard.Operations;

public sealed partial class GetSalesDashboardOverviewOperation
{
    public sealed record Request : IOperationRequest<Response>;
}
