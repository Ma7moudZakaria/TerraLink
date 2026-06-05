using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.OpenApi;
using Microsoft.Extensions.DependencyInjection;

namespace TerraLink.UseCase.Document;

public static class DocumentUseCaseExtensions
{
    public static IServiceCollection AddDocumentUseCase(this IServiceCollection services)
    {
        services.AddValidators<IAttachmentScanner>();
        services.AddOperations<IAttachmentScanner>();

        services.AddOpenApiDoc("document-module", c => c
            .WithTitle("Document API")
            .WithVersion("v1")
            .WithSecurity(s => s.AutoDetect())
            .WithOperationSecurity()
            .WithGroupName("document-module"));

        return services;
    }
}
