using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.OpenApi;
using Microsoft.Extensions.DependencyInjection;

namespace TerraLink.UseCase.Sales
{
    public static class SalesUseCaseExtensions
    {
        public static IServiceCollection AddSalesUseCase(this IServiceCollection services)
        {
            services.AddOperations<ISalesScanner>();
            services.AddManualMapper<ISalesScanner>();

            services.AddOpenApiDoc("sales-module", config =>
            {
                config.WithTitle("Sales API")
                      .WithVersion("v1")
                      .WithSecurity(sec => sec.AutoDetect())
                      .WithOperationSecurity()
                      .WithGroupName("sales-module");
            });

            return services;
        }
    }
}
