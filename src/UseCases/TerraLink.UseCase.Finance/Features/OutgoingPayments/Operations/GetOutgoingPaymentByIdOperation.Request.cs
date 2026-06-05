using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.OutgoingPayments.Specifications;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Operations;

public sealed partial class GetOutgoingPaymentByIdOperation
{
    public sealed record Request(Guid Id) : IOperationRequest<Response>;
}
