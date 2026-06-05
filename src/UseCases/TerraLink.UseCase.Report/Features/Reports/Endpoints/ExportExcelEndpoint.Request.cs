using Microsoft.AspNetCore.Mvc;

namespace TerraLink.UseCase.Report.Features.Reports.Endpoints;

public sealed partial class ExportExcelEndpoint
{
    public sealed class Request
    {
        [FromQuery(Name = "id")] public required Guid LookupSetId { get; set; }
    }
}
