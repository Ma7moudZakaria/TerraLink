using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TerraLink.UseCase.Finance.Features.IncomingPayments.Endpoints;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments;

public sealed class IncomingPaymentsModule : IModule
{
    public static void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/incoming-payments")
            .WithTags("Incoming Payments")
            .WithGroupName("finance-module")
            .RequireAuthorization();

        group.MapEndpoint<CreateIncomingPaymentEndpoint>()
             .MapEndpoint<UpdateIncomingPaymentEndpoint>()
             .MapEndpoint<DeleteIncomingPaymentEndpoint>()
             .MapEndpoint<GetIncomingPaymentsEndpoint>()
             .MapEndpoint<GetContractInstallmentsPaymentStatusEndpoint>()
             .MapEndpoint<GetIncomingPaymentByIdEndpoint>();
    }

    public static void AddServices(IServiceCollection services) { }
}
