using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.OpenApi;
using Microsoft.Extensions.DependencyInjection;

namespace TerraLink.UseCase.Asset
{
    public static class AssetUseCaseExtensions
    {
        public static IServiceCollection AddAssetUseCase(this IServiceCollection services)
        {
            services.AddValidators<IAssetScanner>();
            services.AddOperations<IAssetScanner>();
            services.AddManualMapper<IAssetScanner>();

            services.AddDataProtection();

            services.AddOpenApiDoc("asset-module", config =>
            {
                config.WithTitle("Assets API")
                      .WithVersion("v1")
                      .WithSecurity(sec => sec.AutoDetect())
                      .WithOperationSecurity()
                      .WithGroupName("asset-module");
            });

            return services;
        }
    }
}
