using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.IncomingPayments.Specifications;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Operations;

public sealed partial class GetIncomingPaymentByIdOperation
{
    public sealed record Request(Guid Id) : IOperationRequest<Response>;
}
