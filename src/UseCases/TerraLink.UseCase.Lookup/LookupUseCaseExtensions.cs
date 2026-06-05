using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.OpenApi;
using Microsoft.Extensions.DependencyInjection;

namespace TerraLink.UseCase.Lookup;

public static class LookupUseCaseExtensions
{
    public static IServiceCollection AddLookupUseCase(this IServiceCollection services)
    {
        services.AddValidators<ILookupScanner>();
        services.AddOperations<ILookupScanner>();
        services.AddManualMapper<ILookupScanner>();

        services.AddOpenApiDoc("lookup-module", c => c
            .WithTitle("Lookups API")
            .WithVersion("v1")
            .WithSecurity(s => s.AutoDetect())
            .WithOperationSecurity()
            .WithGroupName("lookup-module"));

        return services;
    }
}
