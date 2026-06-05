using ErrorOr;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.IdentityShield.Features.Auth.Endpoints;
using TerraLink.UseCase.IdentityShield.Features.Auth.Operations;

namespace TerraLink.UseCase.IdentityShield.Features.Auth.Endpoints
{
    public sealed partial class LoginEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapPost("/login", Handle)
           .Accepts<Request>("application/json")
     .Produces<Response>(StatusCodes.Status200OK)
     .ProducesProblem(StatusCodes.Status400BadRequest)
       .ProducesProblem(StatusCodes.Status401Unauthorized)
   .WithName("Login")
  .WithSummary("User Login")
     .WithDescription("Authenticate user and create session")
  .AllowAnonymous()
      .AddLogging();
        }

        public static async Task<IResult> Handle([FromBody] Request request, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new LoginOperation.Request(request), cancellationToken);

            if (result.IsError)
            {
                return result.FirstError.ToUnprocessableEntity();
            }

                        Response response = ObjectMapper.Map<LoginOperation.Response, Response>(result.Value);

            return TypedResults.Ok(response);
        }
    }
}
