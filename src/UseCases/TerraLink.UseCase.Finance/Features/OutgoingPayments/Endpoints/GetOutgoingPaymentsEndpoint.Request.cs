using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Mvc;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Endpoints;

public sealed partial class GetOutgoingPaymentsEndpoint
{
    public sealed class Request : PagedQueryBase
    {
        [FromQuery] public string? Code { get; set; }
        [FromQuery] public string? UnitCode { get; set; }
        [FromQuery] public string? BuildingCode { get; set; }
        [FromQuery] public Guid? ExpenseTypeId { get; set; }
        [FromQuery] public Guid? BeneficiaryId { get; set; }
        [FromQuery] public bool? IsUnitRelated { get; set; }
        [FromQuery] public decimal? Amount { get; set; }
        [FromQuery] public DateTime? PaymentDate { get; set; }
    }
}
