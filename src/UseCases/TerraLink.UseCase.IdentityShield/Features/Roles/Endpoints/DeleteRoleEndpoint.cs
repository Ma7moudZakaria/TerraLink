using ErrorOr;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.IdentityShield.Features.Roles.Operations;

namespace TerraLink.UseCase.IdentityShield.Features.Roles.Endpoints
{
    public sealed partial class DeleteRoleEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapDelete("/{RoleId}", Handle)
              .Produces<Response>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status404NotFound)
           .ProducesProblem(StatusCodes.Status409Conflict)
          .WithName("DeleteRole")
      .WithSummary("Delete Role")
      .WithDescription("Delete a role if it has no assigned users")
        .AllowAnonymous()
              .AddLogging();
        }

        public static async Task<IResult> Handle([FromRoute] Guid RoleId, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new DeleteRoleOperation.Request(RoleId), cancellationToken);

            if (result.IsError)
            {
                return result.FirstError.ToUnprocessableEntity();
            }
            return TypedResults.Ok(Response.Default);
        }
    }
}
