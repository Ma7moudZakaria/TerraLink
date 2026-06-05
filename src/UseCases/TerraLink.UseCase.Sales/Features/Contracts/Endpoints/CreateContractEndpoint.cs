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
using TerraLink.UseCase.Sales.Features.Contracts.Operations;

namespace TerraLink.UseCase.Sales.Features.Contracts.Endpoints
{
    public sealed partial class CreateContractEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapPost("/create", Handle)
               .Produces<Response>(StatusCodes.Status200OK)
               .AddLogging()
               .WithName("CreateContract")
               .WithSummary("Create Contract")
               .RequirePermission(Permissions.Contracts.Create);
        }

        public static async Task<IResult> Handle([FromBody] Request request,
                                                 IOperation dispatcher,
                                                 CancellationToken cancellationToken)
        {
            var createResult = await dispatcher.ExecuteAsync(new CreateContractOperation.Request(request), cancellationToken);

            if (createResult.IsError)
            {
                return createResult.FirstError.ToUnprocessableEntity();
            }
            return TypedResults.Ok(Response.Default);
        }
    }
}
