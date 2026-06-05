using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.OpenApi;
using Microsoft.Extensions.DependencyInjection;

namespace TerraLink.UseCase.Report;

public static class ReportUseCaseExtensions
{
    public static IServiceCollection AddReportUseCase(this IServiceCollection services)
    {
        services.AddValidators<IExportExcelScanner>();
        services.AddOperations<IExportExcelScanner>();

        services.AddOpenApiDoc("excel-module", c => c
            .WithTitle("Excel API")
            .WithVersion("v1")
            .WithSecurity(s => s.AutoDetect())
            .WithOperationSecurity()
            .WithGroupName("excel-module"));

        return services;
    }
}
