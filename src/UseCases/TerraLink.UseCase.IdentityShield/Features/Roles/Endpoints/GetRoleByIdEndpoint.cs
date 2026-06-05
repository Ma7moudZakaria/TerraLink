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
using TerraLink.UseCase.IdentityShield.Features.Roles.Operations;

namespace TerraLink.UseCase.IdentityShield.Features.Roles.Endpoints
{
    public sealed partial class GetRoleByIdEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/{RoleId}", Handle)
            .Produces<Response>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
             .WithName("GetRoleById")
              .WithSummary("Get Role By Id")
       .WithDescription("Retrieve role details including assigned permissions")
              .RequirePermission(Permissions.Identity.View) // Permission filter
           .AddLogging();
        }

        public static async Task<IResult> Handle([FromRoute] Guid RoleId, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new GetRoleByIdOperation.Request(RoleId), cancellationToken);

            if (result.IsError)
            {
                return result.FirstError.ToUnprocessableEntity();
            }

                        Response response = ObjectMapper.Map<GetRoleByIdOperation.Response, Response>(result.Value);

            return TypedResults.Ok(response);
        }
    }
}
