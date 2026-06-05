using LowCodeHub.MinimalEndpoints.Abstractions;

namespace TerraLink.UseCase.Report.Features.Reports.Operations;

public sealed partial class ExportExcelOperation
{
    public sealed record Request(Guid LookupSetId) : IOperationRequest<ExportExcelOperation.Response>;
}
