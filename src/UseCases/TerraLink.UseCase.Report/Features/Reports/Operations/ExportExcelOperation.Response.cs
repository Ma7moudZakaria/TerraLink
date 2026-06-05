namespace TerraLink.UseCase.Report.Features.Reports.Operations;

public sealed partial class ExportExcelOperation
{
    public sealed record Response(byte[] File, string ContentType, string FileName);
}
