using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.OpenApi;
using Microsoft.Extensions.DependencyInjection;

namespace TerraLink.UseCase.Dashboard;

public static class DashboardUseCaseExtensions
{
    public static IServiceCollection AddDashboardUseCase(this IServiceCollection services)
    {
        services.AddValidators<IDashboardScanner>();
        services.AddOperations<IDashboardScanner>();

        services.AddOpenApiDoc("dashboard-module", c => c
            .WithTitle("Dashboard API")
            .WithVersion("v1")
            .WithSecurity(s => s.AutoDetect())
            .WithOperationSecurity()
            .WithGroupName("dashboard-module"));

        return services;
    }
}
