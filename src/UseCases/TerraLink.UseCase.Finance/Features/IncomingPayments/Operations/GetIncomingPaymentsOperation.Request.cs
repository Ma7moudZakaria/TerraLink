using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.IncomingPayments.Specifications;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Operations;

public sealed partial class GetIncomingPaymentsOperation
{
    public sealed record Request(
    string? Code,
    string? ContractCode,
    string? ClientName,
    string? UnitCode,
    decimal? Amount,
    DateTime? PaymentDate,
    int Page,
    int PageSize) : IOperationRequest<PagedList<Response>>;
}
