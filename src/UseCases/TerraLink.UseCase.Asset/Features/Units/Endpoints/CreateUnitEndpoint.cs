using ErrorOr;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using LowCodeHub.Permissions;
using TerraLink.UseCase.Asset.Features.Units.Operations;

namespace TerraLink.UseCase.Asset.Features.Units.Endpoints
{
    public sealed partial class CreateUnitEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapPost("/create", Handle)
             .Produces<Response>(StatusCodes.Status200OK)
             .AddValidator<Request>()
             .AddLogging()
             .WithName("CreateUnit")
             .WithSummary("Create Unit")
               .RequirePermission(Permissions.Assets.Create); // Permission filter
        }


        public static async Task<IResult> Handle([FromBody] Request request, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var createResult = await dispatcher.ExecuteAsync(new CreateUnitOperation.Request(request), cancellationToken);

            if (createResult.IsError)
                return createResult.FirstError.ToBusinessFailure();
            return TypedResults.Ok(Response.Default);

        }
    }
}
