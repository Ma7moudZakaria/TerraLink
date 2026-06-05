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
    public sealed partial class CreateUserEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapPost("/register", Handle)
               .Accepts<Request>("application/json")
               .Produces<Response>(StatusCodes.Status200OK)
               .ProducesProblem(StatusCodes.Status400BadRequest)
               .WithName("CreateUser")
               .WithSummary("Create User")
               .WithDescription("Create a new system user through the protected administrative registration flow")
               .RequirePermission(Permissions.Identity.Create)
               .AddLogging();
        }

        public static async Task<IResult> Handle([FromBody] Request request, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new CreateUserOperation.Request(request), cancellationToken);

            if (result.IsError)
            {
                return result.FirstError.ToUnprocessableEntity();
            }

                        Response response = ObjectMapper.Map<CreateUserOperation.Response, Response>(result.Value);

            return TypedResults.Ok(response);
        }
    }
}
