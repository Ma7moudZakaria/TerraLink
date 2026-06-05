using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.FollowUpCalls.Endpoints;
using TerraLink.UseCase.Sales.Features.FollowUpCalls.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Sales.Features.FollowUpCalls.Operations;

public sealed partial class GetFollowUpCallDetailsOperation
{
    public sealed record Request(Guid ClientId, Guid Id) : IOperationRequest<Response>;
}
