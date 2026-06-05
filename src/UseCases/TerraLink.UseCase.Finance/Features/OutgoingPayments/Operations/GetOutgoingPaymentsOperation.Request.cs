using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.OutgoingPayments.Specifications;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Operations;

public sealed partial class GetOutgoingPaymentsOperation
{
    public sealed record Request(
    string? Code,
    string? UnitCode,
    string? BuildingCode,
    Guid? ExpenseTypeId,
    Guid? BeneficiaryId,
    bool? IsUnitRelated,
    decimal? Amount,
    DateTime? PaymentDate,
    int Page,
    int PageSize) : IOperationRequest<PagedList<Response>>;
}
