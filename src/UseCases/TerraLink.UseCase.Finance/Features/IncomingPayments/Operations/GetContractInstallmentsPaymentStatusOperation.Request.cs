using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Enums;
using TerraLink.UseCase.Finance.Features.IncomingPayments.Specifications;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Operations;

public sealed partial class GetContractInstallmentsPaymentStatusOperation
{
    public sealed record Request(Guid ContractId)
    : IOperationRequest<List<Response>>;
}
