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
    public sealed partial class RefreshTokenEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapPost("/refresh-token", Handle)
                .Accepts<Request>("application/json")
            .Produces<Response>(StatusCodes.Status200OK)
               .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithName("RefreshToken")
          .WithSummary("Refresh Access Token")
        .WithDescription("Generate new access and refresh tokens using a valid refresh token")
          .AllowAnonymous()
            .AddLogging();
        }

        public static async Task<IResult> Handle([FromBody] Request request, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new RefreshTokenOperation.Request(request.RefreshToken), cancellationToken);

            if (result.IsError)
            {
                return result.FirstError.ToUnprocessableEntity();
            }

                        Response response = ObjectMapper.Map<RefreshTokenOperation.Response, Response>(result.Value);

            return TypedResults.Ok(response);
        }
    }
}
