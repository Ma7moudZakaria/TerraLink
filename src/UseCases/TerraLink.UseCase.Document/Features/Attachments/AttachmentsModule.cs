using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TerraLink.UseCase.Document.Features.Attachments.Endpoints;

namespace TerraLink.UseCase.Document.Features.Attachments;

public sealed class AttachmentsModule : IModule
{
    public static void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/documents")
            .WithTags("Documents")
            .WithGroupName("document-module")
            .RequireAuthorization();

        group.MapEndpoint<UploadAttachmentEndpoint>()
             .MapEndpoint<DownloadAttachmentEndpoint>();
    }

    public static void AddServices(IServiceCollection services) { }
}
