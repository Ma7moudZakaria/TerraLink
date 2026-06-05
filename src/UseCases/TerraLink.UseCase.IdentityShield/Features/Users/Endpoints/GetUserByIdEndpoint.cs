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
    public sealed partial class GetUserByIdEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/{UserId}", Handle)
               .Produces<Response>(StatusCodes.Status200OK)
               .ProducesProblem(StatusCodes.Status404NotFound)
               .WithName("GetUserById")
               .WithSummary("Get User By Id")
               .WithDescription("Retrieve system user details by identifier")
               .RequirePermission(Permissions.Identity.View)
               .AddLogging();
        }

        public static async Task<IResult> Handle([FromRoute] Guid UserId, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new GetUserByIdOperation.Request(UserId), cancellationToken);

            if (result.IsError)
            {
                return result.FirstError.ToUnprocessableEntity();
            }

                        Response response = ObjectMapper.Map<GetUserByIdOperation.Response, Response>(result.Value);

            return TypedResults.Ok(response);
        }
    }
}
