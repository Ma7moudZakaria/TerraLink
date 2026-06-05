using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Mvc;

namespace TerraLink.UseCase.Finance.Features.Installments.Endpoints;

public sealed partial class GetInstallmentsEndpoint
{
    public sealed class Request : PagedQueryBase
    {
        [FromQuery] public Guid? ContractId { get; set; }
        [FromQuery] public string? ClientName { get; set; }
        [FromQuery] public string? Unit { get; set; }
    }
}
