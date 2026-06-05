using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.Installments.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Finance.Features.Installments.Operations;

public sealed partial class GetInstallmentsOperation
{
    public sealed record Request(Guid? ContractId, string? ClientName, string? Unit, int Page, int PageSize) : IOperationRequest<PagedList<Response>>;
}
