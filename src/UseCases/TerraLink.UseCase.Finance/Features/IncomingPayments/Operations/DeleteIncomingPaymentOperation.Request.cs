using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.IncomingPayments.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Operations;

public sealed partial class DeleteIncomingPaymentOperation
{
    public sealed record Request(Guid Id) : IOperationRequest<Success>;
}
