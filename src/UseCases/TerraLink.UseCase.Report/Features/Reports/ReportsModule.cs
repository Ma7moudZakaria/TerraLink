using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TerraLink.UseCase.Report.Features.Reports.Endpoints;

namespace TerraLink.UseCase.Report.Features.Reports;

public sealed class ReportsModule : IModule
{
    public static void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/export")
            .WithTags("Reports")
            .WithGroupName("excel-module")
            .RequireAuthorization();

        group.MapEndpoint<ExportExcelEndpoint>();
    }

    public static void AddServices(IServiceCollection services) { }
}
