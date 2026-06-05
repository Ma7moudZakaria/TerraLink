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
    public sealed partial class LogoutEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapPost("/logout", Handle)
           .Accepts<RefreshTokenEndpoint.Request>("application/json")
              .Produces<Response>(StatusCodes.Status200OK)
             .WithName("Logout")
                  .WithSummary("User Logout")
             .WithDescription("Revoke refresh token and end session")
              .RequireAuthorization()
                  .AddLogging();
        }

        public static async Task<IResult> Handle([FromBody] RefreshTokenEndpoint.Request request, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new LogoutOperation.Request(request.RefreshToken), cancellationToken);

            if (result.IsError)
            {
                return result.FirstError.ToUnprocessableEntity();
            }
            return TypedResults.Ok(Response.Default);
        }
    }
}
