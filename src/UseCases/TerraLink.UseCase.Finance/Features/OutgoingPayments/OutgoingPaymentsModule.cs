using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TerraLink.UseCase.Finance.Features.OutgoingPayments.Endpoints;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments;

public sealed class OutgoingPaymentsModule : IModule
{
    public static void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/outgoing-payments")
            .WithTags("Outgoing Payments")
            .WithGroupName("finance-module")
            .RequireAuthorization();

        group.MapEndpoint<CreateOutgoingPaymentEndpoint>()
             .MapEndpoint<UpdateOutgoingPaymentEndpoint>()
             .MapEndpoint<DeleteOutgoingPaymentEndpoint>()
             .MapEndpoint<GetOutgoingPaymentsEndpoint>()
             .MapEndpoint<GetOutgoingPaymentByIdEndpoint>();
    }

    public static void AddServices(IServiceCollection services) { }
}
