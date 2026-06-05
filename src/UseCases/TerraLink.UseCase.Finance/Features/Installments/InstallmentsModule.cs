using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TerraLink.UseCase.Finance.Features.Installments.Endpoints;

namespace TerraLink.UseCase.Finance.Features.Installments;

public sealed class InstallmentsModule : IModule
{
    public static void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/installments")
            .WithTags("Installments")
            .WithGroupName("finance-module")
            .RequireAuthorization();

        group.MapEndpoint<CreateInstallmentEndpoint>()
             .MapEndpoint<UpdateInstallmentEndpoint>()
             .MapEndpoint<GetInstallmentsEndpoint>()
             .MapEndpoint<GetInstallmentByIdEndpoint>();
    }

    public static void AddServices(IServiceCollection services) { }
}
