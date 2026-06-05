using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.OpenApi;
using Microsoft.Extensions.DependencyInjection;

namespace TerraLink.UseCase.Finance;

public static class FinanceUseCaseExtensions
{
    public static IServiceCollection AddFinanceUseCase(this IServiceCollection services)
    {
        services.AddValidators<IFinanceScanner>();
        services.AddOperations<IFinanceScanner>();
        services.AddManualMapper<IFinanceScanner>();

        services.AddOpenApiDoc("finance-module", c => c
            .WithTitle("Finance API")
            .WithVersion("v1")
            .WithSecurity(s => s.AutoDetect())
            .WithOperationSecurity()
            .WithGroupName("finance-module"));

        return services;
    }
}
