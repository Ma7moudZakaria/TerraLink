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
    public sealed partial class UpdateUserEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapPut("/{UserId}", Handle)
               .Accepts<Request>("application/json")
               .Produces<Response>(StatusCodes.Status200OK)
               .ProducesProblem(StatusCodes.Status400BadRequest)
               .ProducesProblem(StatusCodes.Status404NotFound)
               .WithName("UpdateUser")
               .WithSummary("Update User")
               .WithDescription("Update an existing system user")
               .RequirePermission(Permissions.Identity.Edit)
               .AddLogging();
        }

        public static async Task<IResult> Handle([FromRoute] Guid UserId, [FromBody] Request request, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new UpdateUserOperation.Request(UserId, request), cancellationToken);

            if (result.IsError)
            {
                return result.FirstError.ToUnprocessableEntity();
            }
            return TypedResults.Ok(Response.Default);
        }
    }
}
