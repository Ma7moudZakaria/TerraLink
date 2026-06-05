using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.OutgoingPayments.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Operations;

public sealed partial class DeleteOutgoingPaymentOperation
{
    public sealed record Request(Guid Id) : IOperationRequest<Success>;
}
