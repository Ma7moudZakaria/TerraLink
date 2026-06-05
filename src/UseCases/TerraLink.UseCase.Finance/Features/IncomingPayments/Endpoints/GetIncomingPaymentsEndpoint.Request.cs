using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Mvc;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Endpoints;

public sealed partial class GetIncomingPaymentsEndpoint
{
    public sealed class Request : PagedQueryBase
    {
        [FromQuery] public string? Code { get; set; }
        [FromQuery] public string? ContractCode { get; set; }
        [FromQuery] public string? ClientName { get; set; }
        [FromQuery] public string? UnitCode { get; set; }
        [FromQuery] public decimal? Amount { get; set; }
        [FromQuery] public DateTime? PaymentDate { get; set; }
    }
}
