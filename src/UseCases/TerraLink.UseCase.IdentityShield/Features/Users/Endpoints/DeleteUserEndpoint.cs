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
using TerraLink.UseCase.IdentityShield.Features.Users.Operations;

namespace TerraLink.UseCase.IdentityShield.Features.Users.Endpoints
{
    public sealed partial class DeleteUserEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapDelete("/{UserId}", Handle)
               .Produces<Response>(StatusCodes.Status200OK)
               .ProducesProblem(StatusCodes.Status404NotFound)
               .ProducesProblem(StatusCodes.Status409Conflict)
               .WithName("DeleteUser")
               .WithSummary("Delete User")
               .WithDescription("Delete a system user")
               .RequirePermission(Permissions.Identity.Delete)
               .AddLogging();
        }

        public static async Task<IResult> Handle([FromRoute] Guid UserId, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new DeleteUserOperation.Request(UserId), cancellationToken);

            if (result.IsError)
            {
                return result.FirstError.ToUnprocessableEntity();
            }
            return TypedResults.Ok(Response.Default);
        }
    }
}
