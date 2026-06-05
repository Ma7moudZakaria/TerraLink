namespace TerraLink.UseCase.Report.Features.Reports.Endpoints;

public sealed partial class ExportExcelEndpoint
{
    public sealed record Response(byte[] File, string ContentType, string FileName);
}
